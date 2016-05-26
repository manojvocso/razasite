<script runat="server">
'=====================================================================================
'= Cardinal Commerce (http://www.cardinalcommerce.com)
'= CentinelUtility.aspx
'= General Utilities used for Thin Client Integrations
'= Version 6.0 03/21/2006
'=====================================================================================

Function determineCardType(Card_Number)
     
	Dim cardType

	cardType = "UNKNOWN"   ' VISA, MASTERCARD, JCB, AMEX, UNKNOWN

	If (Len(Card_Number) = "16" AND Left(Card_Number, 1) = "4") Then
		cardType = "VISA"
	ElseIf (Len(Card_Number) = "13" AND Left(Card_Number, 1) = "5") Then
		cardType = "MASTERCARD"
	ElseIf (Len(Card_Number) = "16" AND Left(Card_Number, 1) = "5") Then
		cardType = "MASTERCARD"
	ElseIf (Len(Card_Number) = "15" AND Left(Card_Number, 4) = "2131") Then
		cardType = "JCB"
	ElseIf (Len(Card_Number) = "15" AND Left(Card_Number, 4) = "1800") Then
		cardType = "JCB"
	ElseIf (Len(Card_Number) = "16" AND Left(Card_Number, 1) = "3") Then
		cardType = "JCB"
	ElseIf (Len(Card_Number) = "15" AND Left(Card_Number, 2) = "34") Then
		cardType = "AMEX"
	ElseIf (Len(Card_Number) = "15" AND Left(Card_Number, 2) = "37") Then
		cardType = "AMEX"
	End If

	determineCardType = cardType   
	 
End Function

</script>
