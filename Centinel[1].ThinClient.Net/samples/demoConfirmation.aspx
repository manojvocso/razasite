<% @Page Language="VB" Explicit="True"%>
<%response.Expires=-1%>
<%response.Buffer=true%>
<%@ import Namespace="CardinalCommerce" %>
<%
'========================================================================================
'=  Cardinal Commerce (http://www.cardinalcommerce.com)
'=  demoConfirmation.aspx
'=  Version 6.0
'=	03/01/2006
'=
'=	This page represents the order confirmation page.  The page displays the shipping 
'=	and order information.  The customer clicks the payment button to finalize the order.
'=
'========================================================================================
%>
<HTML>
<Head>
<Title>Demo - Order Confirmation</Title>

<%
	' Sets session variables if page is posted from demoCheckout.aspx

	If request("mop") = "credit_card" OR request("mop") = "paypal_classic" OR request("mop") = "paypal_direct" OR request("mop") = "secure_e_bill" Or request("mop") = "bill_me_later" Then	

		Dim transactionType

		If request("mop") = "credit_card" Then
			transactionType = "C"
			Session("Centinel_CardNumber") = request("cardNumber")
			Session("Centinel_NameOnCard") = request("nameOnCard")
			Session("Centinel_ExpMonth") = request("expr_mm")
			Session("Centinel_ExpYear") = request("expr_yyyy")
			Session("Centinel_CardCode") = request("cardCode")
		ElseIf request("mop") = "paypal_classic" Then
			transactionType = "P"
		ElseIf request("mop") = "paypal_direct" Then
			transactionType = "C"
			Session("Centinel_CardNumber") = request("cardNumberPP")
			Session("Centinel_NameOnCard") = request("nameOnCardPP")
			Session("Centinel_ExpMonth") = request("expr_mmPP")
			Session("Centinel_ExpYear") = request("expr_yyyyPP")
			Session("Centinel_CardCode") = request("cardCodePP")
		ElseIf request("mop") = "secure_e_bill" Then
			transactionType = "E"
		ElseIf request("mop") = "bill_me_later" Then
			transactionType = "B"
		End If
		
		Session("Centinel_TransactionType") = transactionType
		Session("Centinel_PaymentType") = request("mop")
		Session("Centinel_BillingFirstName") = request("strFirstName")
		Session("Centinel_BillingLastName") = request("strLastName")
		Session("Centinel_BillingName") = request("strFirstName") & " " & request("strLastName")
		Session("Centinel_BillingAddress") = request("strAddress")
		Session("Centinel_BillingCity") = request("strCity")
		Session("Centinel_BillingState") = request("strState")
		Session("Centinel_BillingPostalCode") = request("strPostcode")
		Session("Centinel_BillingCountryCode") = request("strCountryCode")
		Session("Centinel_BillingPhone") = request("strPhone")
		Session("Centinel_ShippingName") = request("shipfirstname") & " " & request("shiplastname")
		Session("Centinel_ShippingFirstName") = request("shipfirstname")
		Session("Centinel_ShippingLastName") = request("shiplastname")
		Session("Centinel_ShippingAddress") = request("shipaddress")
		Session("Centinel_ShippingCity") = request("shipcity")
		Session("Centinel_ShippingState") = request("shipstate")
		Session("Centinel_ShippingPostalCode") = request("shippostal")
		Session("Centinel_ShippingCountryCode") = request("shipcountry")
		Session("Centinel_ShippingPhone") = request("shipphone")
		Session("Centinel_Email") = request("strEmail")
		
	End If
	
%>

<script language="JavaScript">

    function popUp(url) {
		popupWin=window.open(url,"win",'toolbar=0,location=0,directories=0,status=1,menubar=1,scrollbars=1,width=570,height=450');
		self.name = "mainWin"; 
	}

	function editAddress() {
		if (document.checkout.txnType.value == "X")
		{
			document.checkout.action = "demoXCheckout.aspx";
			document.checkout.submit();
		}
		else
		{
			document.checkout.action = "demoCheckout.aspx";
			document.checkout.submit();
		}
	}

	function editPaymentDetails() {
		document.checkout.action = "demoCheckout.aspx";
		document.checkout.submit();
	}

	function editOrderDetails() {
		document.checkout.action = "demoStartPage.aspx";
		document.checkout.submit();
	}

	function SeedCCPA() {
		if(document.checkout.paymentMethod.options[1].selected == true){
		  document.checkout.cardNumber.value = "4000000000000002";
		  document.checkout.cardCode.value = "123";
		}
		else if(document.checkout.paymentMethod.options[2].selected == true){
		  document.checkout.cardNumber.value = "5200000000000007";
		  document.checkout.cardCode.value = "123";
		}
		else if(document.checkout.paymentMethod.options[3].selected == true){
		  document.checkout.cardNumber.value = "5200000000000007";
		  document.checkout.cardCode.value = "123";
		}		
	}


	function SeedPayPalCards() {
		if(document.checkout.paymentMethodPP.options[1].selected == true){
		  document.checkout.cardNumberPP.value = "4000000000000002";
		  document.checkout.cardCodePP.value = "123";
		}
		else if(document.checkout.paymentMethodPP.options[2].selected == true){
		  document.checkout.cardNumberPP.value = "5200000000000007";
		  document.checkout.cardCodePP.value = "123";
		}
		else if(document.checkout.paymentMethodPP.options[3].selected == true){
		  document.checkout.cardNumberPP.value = "378282246310005";
		  document.checkout.cardCodePP.value = "1234";
		}		
		else if(document.checkout.paymentMethodPP.options[4].selected == true){
		  document.checkout.cardNumberPP.value = "6011111111111117";
		  document.checkout.cardCodePP.value = "123";
		}		
	}
	
	
</script>	
</Head>
<BODY>
<table width="100%" border="0" cellpadding="2" cellspacing="0">
  <tr valign="top"> 
    <td width="200" align="right"><img src="images\merchant_logo.gif"/></td>
	<td width="*"></td>
	<td align="left"></td>
  </tr>
  <tr height="7"> 
    <td colspan="3" valign="top" bgcolor="#e0e0e0" height="2">&nbsp;</td>
  </tr>
</table>
<center>
<h2>Checkout Step 2 of 2</h2>
<br/>
<form name="checkout" action="demoCheckoutProcessor.aspx">
		<table width="640" border="0" cellpadding="2" cellspacing="0">
		<tr bgcolor="#990000"><td align="left"><font face='verdana,arial' size=2 color='#FFFFFF'><b>Billing & Shipping Information</b></font></td><td align="right"><input type="button" value="Edit" onclick="editAddress()"></td></tr>

		<% If Session("Centinel_TransactionType") = "X" Then %>

			<tr><td colspan="2" valign=top><center>
			<table width="320" cellpadding=1 cellspacing=1>
				<tr>
					<td colspan="2" align="center"><font face='verdana,arial' size=2 color='#000000'>Shipping Information </font></td>
				</tr>
				<tr bgcolor='#e0e0e0'>
					<td><font face='verdana,arial' size=2 color='#000000'>* Name </font></td>
					<td><font face='verdana,arial' size=2 color='#000000'><%=Session("Centinel_ShippingName")%></font></td>
				</tr>
				<tr bgcolor='#e0e0e0'>
					<td><font face='verdana,arial' size=2 color='#000000'>* Address </font></td>
					<td><font face='verdana,arial' size=2 color='#000000'><%=Session("Centinel_ShippingAddress")%></font></td>
				</tr>
				<tr bgcolor='#e0e0e0'>
					<td><font face='verdana,arial' size=2 color='#000000'>* City </font></td>
					<td><font face='verdana,arial' size=2 color='#000000'><%=Session("Centinel_ShippingCity")%></font></td>
				</tr>
				<tr bgcolor='#e0e0e0'>
					<td><font face='verdana,arial' size=2 color='#000000'>* State </font></td>
					<td><font face='verdana,arial' size=2 color='#000000'><%=Session("Centinel_ShippingState")%></font></td>
				</tr>
				<tr bgcolor='#e0e0e0'>
					<td><font face='verdana,arial' size=2 color='#000000'>* Postal Code </font></td>
					<td><font face='verdana,arial' size=2 color='#000000'><%=Session("Centinel_ShippingPostalCode")%></font></td>
				</tr>
				<tr bgcolor='#e0e0e0'>
					<td><font face='verdana,arial' size=2 color='#000000'>* Country </font></td>
					<td><font face='verdana,arial' size=2 color='#000000'><%=Session("Centinel_ShippingAddress")%></font></td>
				</tr>
				<tr bgcolor='#e0e0e0'>
					<td><font face='verdana,arial' size=2 color='#000000'>* Email </font></td>
					<td><font face='verdana,arial' size=2 color='#000000'><%=Session("Centinel_Email")%></font></td>
				</tr>
			</table>
			</td></tr>

		<% Else %>

			<tr><td valign=top><center>
				<table width="320" cellpadding=1 cellspacing=1>
				<tr>
					<td colspan="2" align="center"><font face='verdana,arial' size=2 color='#000000'>Billing Information </font></td>
				</tr>
				<tr bgcolor='#e0e0e0'>
					<td><font face='verdana,arial' size=2 color='#000000'>* Name </font></td>
					<td><font face='verdana,arial' size=2 color='#000000'><%=Session("Centinel_BillingName")%></font></td>
				</tr>
				<tr bgcolor='#e0e0e0'><td><font face='verdana,arial' size=2 color='#000000'>* Address </font></td>
					<td><font face='verdana,arial' size=2 color='#000000'><%=Session("Centinel_BillingAddress")%></font></td>
				</tr>
				<tr bgcolor='#e0e0e0'>
					<td><font face='verdana,arial' size=2 color='#000000'>* City </font></td>
					<td><font face='verdana,arial' size=2 color='#000000'><%=Session("Centinel_BillingCity")%></font></td>
				</tr>
				<tr bgcolor='#e0e0e0'>
					<td><font face='verdana,arial' size=2 color='#000000'>* State </font></td>
					<td><font face='verdana,arial' size=2 color='#000000'><%=Session("Centinel_BillingState")%></font></td>
				</tr>
				<tr bgcolor='#e0e0e0'>
					<td><font face='verdana,arial' size=2 color='#000000'>* Postal Code </font></td>
					<td><font face='verdana,arial' size=2 color='#000000'><%=Session("Centinel_BillingPostalCode")%></font></td>
				</tr>
				<tr bgcolor='#e0e0e0'>
					<td><font face='verdana,arial' size=2 color='#000000'>* Country </font></td>
					<td><font face='verdana,arial' size=2 color='#000000'><%=Session("Centinel_BillingCountryCode")%></font></td>
				</tr>			
				<tr bgcolor='#e0e0e0'>
					<td><font face='verdana,arial' size=2 color='#000000'>* Phone</font></td>
					<td><font face='verdana,arial' size=2 color='#000000'><%=Session("Centinel_BillingPhone")%></font></td>
				</tr>
				<tr bgcolor='#e0e0e0'>
					<td><font face='verdana,arial' size=2 color='#000000'>* Email </font></td>
					<td><font face='verdana,arial' size=2 color='#000000'><%=Session("Centinel_Email")%></font></td>
				</tr>
			</table>
			</td>
			<td  valign=top><center>
			<table width="320" cellpadding=1 cellspacing=1>
				<tr>
					<td colspan="2" align="center"><font face='verdana,arial' size=2 color='#000000'>Shipping Information </font></td>
				</tr>
				<tr bgcolor='#e0e0e0'>
					<td><font face='verdana,arial' size=2 color='#000000'>* Name </font></td>
					<td><font face='verdana,arial' size=2 color='#000000'><%=Session("Centinel_ShippingName")%></font></td>
				</tr>
				<tr bgcolor='#e0e0e0'>
					<td><font face='verdana,arial' size=2 color='#000000'>* Address </font></td>
					<td><font face='verdana,arial' size=2 color='#000000'><%=Session("Centinel_ShippingAddress")%></font></td>
				</tr>
				<tr bgcolor='#e0e0e0'>
					<td><font face='verdana,arial' size=2 color='#000000'>* City </font></td>
					<td><font face='verdana,arial' size=2 color='#000000'><%=Session("Centinel_ShippingCity")%></font></td>
				</tr>
				<tr bgcolor='#e0e0e0'>
					<td><font face='verdana,arial' size=2 color='#000000'>* State </font></td>
					<td><font face='verdana,arial' size=2 color='#000000'><%=Session("Centinel_ShippingState")%></font></td>
				</tr>
				<tr bgcolor='#e0e0e0'>
					<td><font face='verdana,arial' size=2 color='#000000'>* Postal Code </font></td>
					<td><font face='verdana,arial' size=2 color='#000000'><%=Session("Centinel_ShippingPostalCode")%></font></td>
				</tr>
				<tr bgcolor='#e0e0e0'>
					<td><font face='verdana,arial' size=2 color='#000000'>* Country </font></td>
					<td><font face='verdana,arial' size=2 color='#000000'><%=Session("Centinel_ShippingCountryCode")%></font></td>
				</tr>
				<tr bgcolor='#e0e0e0'>
					<td><font face='verdana,arial' size=2 color='#000000'>* Phone</font></td>
					<td><font face='verdana,arial' size=2 color='#000000'><%=Session("Centinel_ShippingPhone")%></font></td>
				</tr>
			</table>
			</td></tr>
		<% End If %>
		<tr><td colspan="2" align="center">&nbsp;</td></tr>
		<tr bgcolor="#990000"><td align="left">
		         <font face='verdana,arial' size=2 color='#FFFFFF'><b>Order Details</b></font>
		                      </td><td align="right">
		         <input type="button" value="Edit" onclick="editOrderDetails();">
		                           </td></tr>
		<tr><td colspan="2" align="center">
			<table width="640" border='1' cellpadding='2' cellspacing='0' bordercolor='#e0e0e0'>
			<tr bgcolor='#FFFFFF'>
				<td><p align=center><b><font color='#000000' face='Arial,Verdana' size='2'>Description </font></b></td>
				<td><p align=center><b><font color='#000000' face='Arial,Verdana' size='2'>Quantity </font></b></td>
				<td><p align=center><b><font color='#000000' face='Arial,Verdana' size='2'>Unit Price </font></b></td>
				<td><p align=center><b><font color='#000000' face='Arial,Verdana' size='2'>Total</font></b></td>
			</tr>
			<tr bgcolor='#FFFFFF'>
				<td align="left" width="50%"><font face='verdana,arial' size=2><%=Session("Item_Description")%></td>
				<td width='10%' align=center><%=Session("Item_Quantity")%></td>
				<td align="right" width="10%"><font face='verdana,arial' size=2>$<%=Session("Formatted_Amount")%></font></td>
				<td align="right" width="10%"><font face='verdana,arial' size=2>$<%=Session("Formatted_Amount")%></font></td>
			</tr>
			<tr bgcolor='#FFFFFF'>
				<td align="right" colspan="3"><font face='verdana,arial' size=2>Order Total</td>
				<td align="right"><font face='verdana,arial' size=2>$<%=Session("Formatted_Amount")%></font></td>
			</tr>
			</table>

		</td></tr>
		<tr><td colspan="2" align="center">&nbsp;</td></tr>
		<tr bgcolor="#990000"><td align="left"><font face='verdana,arial' size=2 color='#FFFFFF'><b>Payment Information</b></font></td><td align="right"><input type="button" value="Edit" onclick="editPaymentDetails();"></td></tr>
		<tr><td colspan="2" align="center">
			<% If Session("Centinel_PaymentType") = "credit_card" Then %>
				<table width="550" border="0" cellpadding="0" cellspacing="0">
				<tr>
				<td width="150"><font face='verdana,arial' size=2 color='#000000'>* Card Type</font></td>
				<td width="400">
					<select name="paymentMethod" id="paymentMethod" onChange="SeedCCPA()">
						<option value="">Select Card Type</option>
						<option value="visa">Visa</option>
						<option value="mc">Mastercard</option>
						<option value="mc">Maestro</option>
				</td></tr>
				<tr><td><font face='verdana,arial' size=2 color='#000000'>* Name on Card </font></td><td><input size=35 name="nameOnCard" id="nameOnCard" value="Joe Shopper"></td></tr>
				<tr><td><font face='verdana,arial' size=2 color='#000000'>* Card Number </font></td><td><input maxLength=20 size=17 name="cardNumber" id="cardNumber" value=""> <img src='images/cc_visa.gif'/> <img src='images/cc_mastercard.gif'/></td></tr>
				
				<tr><td><font face='verdana,arial' size=2 color='#000000'>* Expiration Date</font></td><td>
				<select name="expr_mm" id="expr_mm">
						<option value="01">01</option>
						<option value="02">02</option>
						<option value="03">03</option>
						<option value="04">04</option>
						<option value="05">05</option>
						<option value="06">06</option>
						<option value="07">07</option>
						<option value="08">08</option>
						<option value="09">09</option>
						<option value="10">10</option>
						<option value="11">11</option>
						<option value="12">12</option>
				</select>
				<select name="expr_yyyy" id="expr_yyyy">
						<option value="2013" selected>2013</option>
						<option value="2014">2014</option>
						<option value="2015">2015</option>
						<option value="2016">2016</option>
						<option value="2017">2017</option>
				</select>
				</td></tr>
				<tr><td><font face='verdana,arial' size=2 color='#000000'>* Card Code </font></td><td><input size=5 name="cardCode" id="cardCode" value="123"></td></tr>
				<tr><td colspan='2'>
					<table width='640' border='0' cellpadding='4'><tr><td><font face='verdana,arial,helvetica' size=2 color='#BB0000'> Your card may be eligible or enrolled in Verified by Visa™ or MasterCard® SecureCode™ payer authentication programs. After clicking the 'Submit Order' button, your Card Issuer may prompt you for your payer authentication password to complete your purchase.</font></td><td valign='center'><a href='javascript:popUp("mcs_learn_more.html")'><img src='images/mcsc_learn_more.gif' border='0' /></a></td><td valign='center'><a href='javascript:popUp("vbv_learn_more.html")'><img src='images/vbv_learn_more.gif' border='0'/></a></td></tr>
					</table>
					
				</td></tr>
				</table>

			<% ElseIf Session("Centinel_PaymentType") = "paypal_direct" Then%>
				<table width="100%" border="0" cellpadding="0" cellspacing="0">
				<tr>
				<td width="150"><font face='verdana,arial' size=2 color='#000000'>* Card Type</font></td>
				<td width="400">
					<select name="paymentMethodPP" id="paymentMethodPP" onChange="SeedPayPalCards()">
						<option value="">Select Card Type</option>
						<option value="visa">Visa</option>
						<option value="mc">Mastercard</option>
						<option value="amex">American Express</option>
						<option value="discover">Discover</option>
				</td></tr>
				<tr><td><font face='verdana,arial' size=2 color='#000000'>* Name on Card </font></td><td><input size=35 name="nameOnCardPP" id="nameOnCardPP" value="Joe Shopper"></td></tr>
				<tr><td><font face='verdana,arial' size=2 color='#000000'>* Card Number </font></td><td><input maxLength=20 size=17 name="cardNumberPP" id="cardNumberPP" value=""> <img src='images/cc_visa.gif'/> <img src='images/cc_mastercard.gif'/> <img src='images/cc_discover.gif'/> <img src='images/cc_amex.gif'/></td></tr>
				
				<tr ><td><font face='verdana,arial' size=2 color='#000000'>* Expiration Date</font></td><td>
				<select name="expr_mmPP" id="expr_mmPP">
						<option value="01">01</option>
						<option value="02">02</option>
						<option value="03">03</option>
						<option value="04">04</option>
						<option value="05">05</option>
						<option value="06">06</option>
						<option value="07">07</option>
						<option value="08">08</option>
						<option value="09">09</option>
						<option value="10">10</option>
						<option value="11">11</option>
						<option value="12">12</option>
				</select>
				<select name="expr_yyyyPP" id="expr_yyyyPP">
						<option value="2005">2005</option>
						<option value="2006">2006</option>
						<option value="2007">2007</option>
						<option value="2008" selected>2008</option>
						<option value="2009">2009</option>
				</select>
				</td></tr>
				<tr><td><font face='verdana,arial' size=2 color='#000000'>* Card Code </font></td><td><input size=5 name="cardCodePP" id="cardCodePP" value="123"></td></tr>
				</table>
			<% ElseIf Session("Centinel_PaymentType") = "paypal_classic" Then %>
				<img src='images/pay_with_paypal.gif' border='0'/><br>You have selected to use PayPal to complete this purchase. Once your click the 'Submit Order' button you will be transfered to PayPal to complete this order.
			<% ElseIf Session("Centinel_PaymentType") = "secure_e_bill" Then %>
				<img src='images/seb_image.gif' border='0'/><br>You have selected to use SECURE-eBill to complete this purchase. Once your click the 'Submit Order' button you will be presented with your SECURE-eBill order confirmation. 
				<br/><br/>Complete the form and your eBill will be sent to email address specified to complete this order.
			<% ElseIf Session("Centinel_PaymentType") = "bill_me_later" Then %>
				<img src='images/bml_logo_horizontal.jpg' border='0'/><br>You have selected to use Bill Me Later<sup>&reg;</sup> to complete this purchase. Once you click the 'Submit Order' button you will be presented with a Bill Me Later<sup>&reg;</sup> application form. 
				<br/><br/>You may be eligible for a promotional offer:
				<br/><input type="checkbox" checked="checked" name="promoCode" id="promoCode">Yes, I'd like no payments for 90 days on purchases over $250.</input>
				<br/><br/>
		    <% Else %>
				You have selected to use your PayPal account to complete this purchase.<br/><b><%=Session("Centinel_Email")%></b>
			<% End If%>
		
		</td></tr>
		<tr bgcolor="#990000"><td colspan="2" align="center">&nbsp;</td></tr>
		<tr><td colspan="2" align="center">Confirm your order information and click the 'Submit Order' button to complete your purchase.</td></tr>
		<tr><td colspan="2" align="center"><input type="hidden" name="txnType" value="<%=Session("Centinel_TransactionType")%>"/>
		<input type="submit" value="Submit Order" name="PayButton" ID="Submit1"/>
		</td></tr>
	</table>
</form>

</body>
</HTML>








