Imports System.Collections.Generic
Imports System.Linq 'DC 21JAN16 - this needed to be added to support new curve fitting, specifically the ToList part of line "Return answer.ToList()"
Module CurveFunctions
    ' The function.
    Public Function F(ByVal coeffs As List(Of Double), ByVal x As Double) As Double
        Dim total As Double = 0
        Dim x_factor As Double = 1
        For i As Integer = 0 To coeffs.Count - 1
            total += x_factor * coeffs(i)
            x_factor *= x
        Next i
        Return total
    End Function

    ' Return the error squared.
    Public Function ErrorSquared(ByVal points As List(Of PointF), ByVal coeffs As List(Of Double)) As Double
        Dim total As Double = 0
        For Each pt As PointF In points
            Dim dy As Double = pt.Y - F(coeffs, pt.X)
            total += dy * dy
        Next pt
        Return total
    End Function

    ' Find the least squares linear fit.
    'DC 21JAN16 - FindPolynomialLeastSquaresFit needs to be modified to handle the original SimpleDyno arrays of raw data
    'This is the original function
    Public Function FindPolynomialLeastSquaresFit(ByVal points As List(Of PointF), ByVal degree As Integer) As List(Of Double)
        ' Allocate space for (degree + 1) equations with 
        ' (degree + 2) terms each (including the constant term).
        Dim coeffs(degree, degree + 1) As Double

        ' Calculate the coefficients for the equations.
        For j As Integer = 0 To degree
            ' Calculate the coefficients for the jth equation.

            ' Calculate the constant term for this equation.
            coeffs(j, degree + 1) = 0
            For Each pt As PointF In points
                coeffs(j, degree + 1) -= Math.Pow(pt.X, j) * pt.Y
            Next pt

            ' Calculate the other coefficients.
            For a_sub As Integer = 0 To degree
                ' Calculate the dth coefficient.
                coeffs(j, a_sub) = 0
                For Each pt As PointF In points
                    coeffs(j, a_sub) -= Math.Pow(pt.X, a_sub + j)
                Next pt
            Next a_sub
        Next j

        ' Solve the equations.
        Dim answer() As Double = GaussianElimination(coeffs)

        ' Return the result converted into a List(Of Double).
        Return answer.ToList()
    End Function
    'This is the revised function for use with SimpleDyno
    Public Function FindPolynomialLeastSquaresFit_NEW(ByRef SentX() As Double, ByRef SentY() As Double, ByRef SentFY() As Double, ByVal degree As Integer) As Boolean

        Dim TempCount As Integer 'added for SD

        ' Allocate space for (degree + 1) equations with 
        ' (degree + 2) terms each (including the constant term).
        Dim coeffs(degree, degree + 1) As Double

        ' Calculate the coefficients for the equations.
        For j As Integer = 0 To degree
            ' Calculate the coefficients for the jth equation.

            ' Calculate the constant term for this equation.
            coeffs(j, degree + 1) = 0
            'DC 21JAN16 - the following loop replaced
            'For Each pt As PointF In points
            ' coeffs(j, degree + 1) -= Math.Pow(pt.X, j) * pt.Y
            'Next pt

            For TempCount = 0 To UBound(SentX)
                coeffs(j, degree + 1) -= Math.Pow(SentX(TempCount), j) * SentY(TempCount)
            Next TempCount


            ' Calculate the other coefficients.
            For a_sub As Integer = 0 To degree
                ' Calculate the dth coefficient.
                coeffs(j, a_sub) = 0
                'DC 21JAN16 The following Loop replaced
                'For Each pt As PointF In points
                'coeffs(j, a_sub) -= Math.Pow(pt.X, a_sub + j)
                'Next pt
                For TempCount = 0 To UBound(SentX)
                    coeffs(j, a_sub) -= Math.Pow(SentX(TempCount), a_sub + j)
                Next TempCount
            Next a_sub
        Next j

        ' Solve the equations.
        Dim answer() As Double = GaussianElimination(coeffs)
        'Original
        'Return the result converted into a List(Of Double).
        'Return answer.ToList()
        'New - Use F function to calculate the fy values

        For TempCount = 0 To UBound(SentX)
            SentFY(TempCount) = F(answer.ToList(), SentX(TempCount))
        Next
        'FOR NOW JUST RETURN TRUE

        Return True
    End Function

    ' Perform Gaussian elimination on these coefficients.
    ' Return the array of values that gives the solution.
    Private Function GaussianElimination(ByVal coeffs(,) As Double) As Double()
        Dim max_equation As Integer = coeffs.GetUpperBound(0)
        Dim max_coeff As Integer = coeffs.GetUpperBound(1)
        For i As Integer = 0 To max_equation
            ' Use equation_coeffs(i, i) to eliminate the ith
            ' coefficient in all of the other equations.

            ' Find a row with non-zero ith coefficient.
            If (coeffs(i, i) = 0) Then
                For j As Integer = i + 1 To max_equation
                    ' See if this one works.
                    If (coeffs(j, i) <> 0) Then
                        ' This one works. Swap equations i and j.
                        ' This starts at k = i because all
                        ' coefficients to the left are 0.
                        For k As Integer = i To max_coeff
                            Dim temp As Double = coeffs(i, k)
                            coeffs(i, k) = coeffs(j, k)
                            coeffs(j, k) = temp
                        Next k
                        Exit For
                    End If
                Next j
            End If

            ' Make sure we found an equation with
            ' a non-zero ith coefficient.
            Dim coeff_i_i As Double = coeffs(i, i)
            If coeff_i_i = 0 Then
                Throw New ArithmeticException(String.Format( _
                    "There is no unique solution for these points.", _
                    coeffs.GetUpperBound(0) - 1))
            End If

            ' Normalize the ith equation.
            For j As Integer = i To max_coeff
                coeffs(i, j) /= coeff_i_i
            Next j

            ' Use this equation value to zero out
            ' the other equations' ith coefficients.
            For j As Integer = 0 To max_equation
                ' Skip the ith equation.
                If (j <> i) Then
                    ' Zero the jth equation's ith coefficient.
                    Dim coef_j_i As Double = coeffs(j, i)
                    For d As Integer = 0 To max_coeff
                        coeffs(j, d) -= coeffs(i, d) * coef_j_i
                    Next d
                End If
            Next j
        Next i

        ' At this point, the ith equation contains
        ' 2 non-zero entries:
        '      The ith entry which is 1
        '      The last entry coeffs(max_coeff)
        ' This means Ai = equation_coef(max_coeff).
        Dim solution(max_equation) As Double
        For i As Integer = 0 To max_equation
            solution(i) = coeffs(i, max_coeff)
        Next i

        ' Return the solution values.
        Return solution
    End Function
End Module
