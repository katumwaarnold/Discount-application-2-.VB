' Replacement for the existing button handler: paste inside the existing form class.
' Assumptions: amounts use two decimal places; midpoint rounding is away from zero.
' The entered price is rounded first, then the discount; the final price is the
' rounded price minus the rounded discount. Currency symbol follows CurrentCulture.
' Input uses the current culture's decimal separator; grouping and exponents are
' intentionally not accepted. Inputs are limited to 28 digits (including zeros)
' so Decimal parsing cannot silently round a value across a validation boundary.
' Percentage precision is limited to four decimal places and price is capped at
' 999999999999999999.99 to keep the Decimal calculation exact to currency units.
' Outputs are cleared on each calculation attempt.
Private Sub btnCalculateDiscount_Click(sender As Object, e As EventArgs) Handles btnCalculateDiscount.Click
    lblDiscountAmount.Text = String.Empty
    lblFinalPrice.Text = String.Empty

    Dim originalPrice As Decimal
    Dim discountPercent As Decimal
    Dim inputStyle As System.Globalization.NumberStyles =
        System.Globalization.NumberStyles.AllowLeadingWhite Or
        System.Globalization.NumberStyles.AllowTrailingWhite Or
        System.Globalization.NumberStyles.AllowLeadingSign Or
        System.Globalization.NumberStyles.AllowDecimalPoint
    Dim culture As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CurrentCulture

    If System.Text.RegularExpressions.Regex.Matches(txtPrice.Text, "[0-9]").Count > 28 OrElse
       System.Text.RegularExpressions.Regex.Matches(txtDiscount.Text, "[0-9]").Count > 28 Then
        MessageBox.Show("Please use no more than 28 digits in each number.")
        Return
    End If

    If Not Decimal.TryParse(txtPrice.Text, inputStyle, culture, originalPrice) OrElse
       Not Decimal.TryParse(txtDiscount.Text, inputStyle, culture, discountPercent) Then
        MessageBox.Show("Please enter valid numbers using the current decimal separator, without grouping separators.")
        Return
    End If

    If originalPrice < 0D Then
        MessageBox.Show("The price cannot be negative.")
        Return
    End If

    If discountPercent < 0D OrElse discountPercent > 100D Then
        MessageBox.Show("The discount percentage must be between 0 and 100.")
        Return
    End If

    If originalPrice > 999999999999999999.99D Then
        MessageBox.Show("The price is too large; the maximum is 999999999999999999.99.")
        Return
    End If

    If discountPercent <> Decimal.Round(discountPercent, 4) Then
        MessageBox.Show("Please use at most four decimal places for the discount percentage.")
        Return
    End If

    Try
        Dim roundedPrice As Decimal = Decimal.Round(originalPrice, 2, MidpointRounding.AwayFromZero)
        Dim discountAmount As Decimal = Decimal.Round(roundedPrice * (discountPercent / 100D), 2, MidpointRounding.AwayFromZero)
        Dim finalPrice As Decimal = roundedPrice - discountAmount

        lblDiscountAmount.Text = discountAmount.ToString("C2", culture)
        lblFinalPrice.Text = finalPrice.ToString("C2", culture)
    Catch ex As OverflowException
        MessageBox.Show("The amount is too large to calculate.")
    End Try
End Sub
