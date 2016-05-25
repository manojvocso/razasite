<% @Page Language="VB" Explicit="True"%>
<%response.Expires=-1%>
<%response.Buffer=true%>
<%@ import Namespace="CardinalCommerce" %>
<%
'========================================================================================
'=  Cardinal Commerce (http://www.cardinalcommerce.com)
'=  demoCheckout.aspx
'=  Version 6.0
'=	03/01/2006
'=
'=  This page captures the customer's information and allows the customer to choose between
'=  the different payment options (credit card authentication, Paypal, Paypal direct payment
'=  with a credit card, or SECURE-eBill).  The information from this page is posted to
'=  the demoCheckoutProcessor.aspx page for processing.
'=
'========================================================================================
%>
<HTML>
<Head>
<Title>Demo - Checkout</Title>
<Script language="JavaScript">


	function popUp(url) {
		popupWin=window.open(url,"win",'toolbar=0,location=0,directories=0,status=1,menubar=1,scrollbars=1,width=570,height=450');
		self.name = "mainWin"; 
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
<br/>
<% If Len(Session("Message")) > 0 then %>
<br/>
	<p><font color="red"><b><%=Session("Message")%></b></font></p>
<br/>
<% 
  Session("Message") = ""
  End If %>
<h2>Checkout Step 1 of 2</h2>
<table width="700" border="0" cellpadding="2" cellspacing="0">
	<tr bgcolor="#990000"><td align="left"><font face='verdana,arial' size=2 color='#FFFFFF'><b>Billing & Shipping Information</b></font></td></tr>
</table>
<br/>
<p><a href="demoXCheckout.aspx"><img border="0" src="images/express_checkout_button.gif"/></a></p>
<form name="checkout" method=Post action="demoConfirmation.aspx">
	<table width="640" border="0" cellpadding="2" cellspacing="2">
		<tr><td valign=top><center>
		<table width="320" cellpadding=1 cellspacing=1>
			<tr>
				<td colspan="2" align="center"><font face='verdana,arial' size=2 color='#000000'>Billing Information </font></td>
			</tr>
			<tr bgcolor='#e0e0e0'>
				<td><font face='verdana,arial' size=2 color='#000000'>* First Name </font></td>
				<td><font face='verdana,arial' size=2 color='#000000'><input size=25 name="strFirstname" value="Joe"></font></td>
			</tr>
			<tr bgcolor='#e0e0e0'>
				<td><font face='verdana,arial' size=2 color='#000000'>* Last Name </font></td>
				<td><font face='verdana,arial' size=2 color='#000000'><input size=25 name="strLastname" value="Shopper"></font></td>
			</tr>
			<tr bgcolor='#e0e0e0'><td><font face='verdana,arial' size=2 color='#000000'>* Address </font></td>
				<td><font face='verdana,arial' size=2 color='#000000'><input size=25 name="strAddress" value="12345 Main Street"></font></td>
			</tr>
			<tr bgcolor='#e0e0e0'>
				<td><font face='verdana,arial' size=2 color='#000000'>* City </font></td>
				<td><font face='verdana,arial' size=2 color='#000000'><input size=25 name="strCity" value="Mentor"></font></td>
			</tr>
			<tr bgcolor='#e0e0e0'>
				<td><font face='verdana,arial' size=2 color='#000000'>* State </font></td>
				<td><font face='verdana,arial' size=2 color='#000000'><input size=25 name="strState" value="OH"></font></td>
			</tr>
			<tr bgcolor='#e0e0e0'>
				<td><font face='verdana,arial' size=2 color='#000000'>* Postal Code </font></td>
				<td><font face='verdana,arial' size=2 color='#000000'><input size=25 name="strPostcode" value="44094"></font></td>
			</tr>
			<tr bgcolor='#e0e0e0'>
				<td><font face='verdana,arial' size=2 color='#000000'>* Country </font></td>
				<td><font face='verdana,arial' size=2 color='#000000'><input size=25 name="strCountrycode" value="US"></font></td>
			</tr>			
			<tr bgcolor='#e0e0e0'>
				<td><font face='verdana,arial' size=2 color='#000000'>* Phone Number </font></td>
				<td><font face='verdana,arial' size=2 color='#000000'><input size=25 name="strPhone" value="4402555555"></font></td>
			</tr>			
			<tr bgcolor='#e0e0e0'>
				<td><font face='verdana,arial' size=2 color='#000000'>* Email </font></td>
				<td><font face='verdana,arial' size=2 color='#000000'><input size=25 name="strEmail" value="testuser@cardinalcommerce.com"></font></td>
			</tr>
		</table>
		</td>
		<td  valign=top><center>
		<table width="320" cellpadding=1 cellspacing=1>
			<tr>
				<td colspan="2" align="center"><font face='verdana,arial' size=2 color='#000000'>Shipping Information </font></td>
			</tr>
			<tr bgcolor='#e0e0e0'>
				<td><font face='verdana,arial' size=2 color='#000000'>* First Name </font></td>
				<td><font face='verdana,arial' size=2 color='#000000'><input size=25 name="shipfirstname" value="Joe"></font></td>
			</tr>
			<tr bgcolor='#e0e0e0'>
				<td><font face='verdana,arial' size=2 color='#000000'>* Last Name </font></td>
				<td><font face='verdana,arial' size=2 color='#000000'><input size=25 name="shiplastname" value="Shopper"></font></td>
			</tr>			
			<tr bgcolor='#e0e0e0'>
				<td><font face='verdana,arial' size=2 color='#000000'>* Address </font></td>
				<td><font face='verdana,arial' size=2 color='#000000'><input size=25 name="shipaddress" value="12345 Main Street"></font></td>
			</tr>
			<tr bgcolor='#e0e0e0'>
				<td><font face='verdana,arial' size=2 color='#000000'>* City </font></td>
				<td><font face='verdana,arial' size=2 color='#000000'><input size=25 name="shipcity" value="Mentor"></font></td>
			</tr>
			<tr bgcolor='#e0e0e0'>
				<td><font face='verdana,arial' size=2 color='#000000'>* State </font></td>
				<td><font face='verdana,arial' size=2 color='#000000'><input size=25 name="shipstate" value="OH"></font></td>
			</tr>
			<tr bgcolor='#e0e0e0'>
				<td><font face='verdana,arial' size=2 color='#000000'>* Postal Code </font></td>
				<td><font face='verdana,arial' size=2 color='#000000'><input size=25 name="shippostal" value="44094"></font></td>
			</tr>
			<tr bgcolor='#e0e0e0'>
				<td><font face='verdana,arial' size=2 color='#000000'>* Country </font></td>
				<td><font face='verdana,arial' size=2 color='#000000'><input size=25 name="shipcountry" value="US"></font></td>
			</tr>
			<tr bgcolor='#e0e0e0'>
				<td><font face='verdana,arial' size=2 color='#000000'>* Phone Number </font></td>
				<td><font face='verdana,arial' size=2 color='#000000'><input size=25 name="shipphone" value="4402555555"></font></td>
			</tr>
			<tr>
				<td colspan="2"><font face='verdana,arial,helvetica' size=2 color='#BB0000'>Fields marked with an * are required</font></td>
			</tr>
		</table>
		</td></tr>
	</table>
	<br/>
	<table width="700" border="0" cellpadding="2" cellspacing="0">
	<tr bgcolor="#990000"><td align="left"><font face='verdana,arial' size=2 color='#FFFFFF'><b>Payment Information</b></font></td></tr>
	<tr><td align="center">Select <b>one</b> of the following payment methods and complete the coresponding payment form.<br/> Click the 'Continue' button at the bottom of the page to complete your transaction.</td></tr>
	</table>
	<br/>
	<table width="640" border="0" cellpadding="2" cellspacing="0">
	<tr bgcolor="#e0e0e0"><td width="100"><input type="radio" name="mop" value="credit_card"></td><td>Credit / Debit Card with Payer Authentication</td></tr>
	<tr><td valign="center"></td>
		<td valign="center">
		<img src='images/cc_visa.gif'/> <img src='images/cc_mastercard.gif'/>
		
			<table width='640' border='0' cellpadding='4'><tr><td><font face='verdana,arial,helvetica' size=2 color='#BB0000'> A safe and secure checkout is our number one priority for our customers. For additional security this site participates in the Verified by Visa and MasterCard SecureCode payer authentication programs.</font></td><td valign='center'><a href='javascript:popUp("mcs_learn_more.html")'><img src='images/mcsc_learn_more.gif' border='0' /></a></td><td valign='center'><a href='javascript:popUp("vbv_learn_more.html")'><img src='images/vbv_learn_more.gif' border='0'/></a></td></tr>
			</table>
			
	</td></tr>
	<tr bgcolor="#e0e0e0"><td width="100"><input type="radio" name="mop" value="bill_me_later"></td><td>Bill Me Later<sup>&reg;</sup></td></tr>
	<tr><td width="10" valign="center"></td>
	<td>
		<table width='640' border='0' cellpadding='4' ID="Table2"><tr><td width="100" valign='center'><img src='images/bmlLogoSmGr.gif' border='0'/></td><td valign='bottom'><a href='javascript:popUp("bml_learn_more.htm")'><img src='images/BMLfastSecuremd_reg.gif' border='0'/></a></td></tr>
		</table>	
	</td></tr> 
	<tr bgcolor="#e0e0e0"><td width="100"><input type="radio" name="mop" value="secure_e_bill"></td><td>SECURE-eBill</td></tr>
	<tr><td width="10" valign="center"></td>
	<td>
		<table width='640' border='0' cellpadding='4'><tr><td valign='center'><img src='images/seb_image.gif' border='0'/></td><td><font face='verdana,arial,helvetica' size=2 color='#BB0000'> SECURE-eBill is an easy to use and secure online payment product. No credit card is required and no personal financial information (PIN, password, etc.) ever needs to be released. All you need is a valid email address and a bank account and you can easily and safely use SECURE-eBill. Security and convenience within your control. <a href='javascript:popUp("seb_learn_more.html")'>Click Here to Learn More</a></font></td></tr>
		</table>
		
	</td></tr> 
	<tr bgcolor="#e0e0e0"><td width="100"><input type="radio" name="mop" value="paypal_classic"></td><td>PayPal Payment</td></tr>
	<tr><td width="10" valign="center"></td>
	<td>
		<table width='640' border='0' cellpadding='4'><tr><td valign='center'><img src='images/pay_with_paypal.gif' border='0'/></td><td><font face='verdana,arial,helvetica' size=2 color='#BB0000'> Complete your payment using PayPal. You will be transfered to the PayPal website to login and complete payment for this order.</font></td></tr>
		</table>
		
	</td></tr> 
	<tr bgcolor="#e0e0e0"><td width="100"><input type="radio" name="mop" value="paypal_direct"></td><td>Credit / Debit Card using PayPal Direct Payment</td></tr>
	<tr><td width="10"  valign="center"></td>
	<td><img src='images/cc_visa.gif'/> <img src='images/cc_mastercard.gif'/> <img src='images/cc_discover.gif'/> <img src='images/cc_amex.gif'/>
	</td></tr>
	<tr bgcolor="#990000"><td colspan="4">&nbsp;</td></tr>
	<tr><td colspan="4" align="center">Select your method of payment and complete the coresponding payment form.<br/> Click the 'Continue' button at the bottom of the page to complete your transaction.</td></tr>
	<tr><td colspan="4" align="center"><input type="submit" name="submit" value="Continue"></td></tr>
	</table>
	
</form>
</center>
</body>
</html>