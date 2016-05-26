<% @Page Language="VB" Explicit="True"%>
<%response.Expires=-1%>
<%response.Buffer=true%>
<%@ import Namespace="CardinalCommerce" %>
<%
'========================================================================================
'=  Cardinal Commerce (http://www.cardinalcommerce.com)
'=  demoStartPage.aspx
'=  Version 6.0
'=	03/01/2006
'=
'=  This page acts as the final page before the customer checks out.  This page displays
'=  the items in the shopping cart and allows the customer the option of choosing a normal
'=  checkout or checking out via paypal express checkout.
'=
'========================================================================================
%>
<HTML>
<Head>
<Title>Demo - Start Page</Title>
<Script language="JavaScript">


	function popUp(url) {
		popupWin=window.open(url,"win",'toolbar=0,location=0,directories=0,status=1,menubar=1,scrollbars=1,width=570,height=450');
		self.name = "mainWin"; 
	}

	
</script>
</Head>
<BODY>
<%

' Store order information into session variables for later use

    Session("Centinel_TransactionAmount") = "10"
Session("Centinel_OrderNumber") = "1234512345"
    Session("Formatted_Amount") = "1.00"
Session("Item_Quantity") = "1"
Session("Item_Description") = "White Polo Shirt"

%>
<table width="100%" border="0" cellpadding="2" cellspacing="0">
  <tr valign="top"> 
    <td width="200" align="right"><img src="images\merchant_logo.gif"/></td>
	<td width="*"></td>
	<td align="left"></td>
  </tr>
  <tr height="7"> 
    <td colspan="3" valign="top" bgcolor="#e0e0e0" height="2">&nbsp;</td>
  </tr>
  <tr height="7"> 
    <td colspan="3" align="center" valign="top" bgcolor="#ffffff" height="2"><a href='javascript:popUp("bml_learn_more.htm")'><img src="images\bmltitle.gif" border='0'/></a></td>
  </tr>
</table>

<h2 align='center'>Shopping Cart</h2>


<form action="demoCheckout.aspx" method="POST">
<div align='center'>
The following item(s) are currently in your cart.

<table border='1' cellpadding='2' cellspacing='0' width='70%' bordercolor='#e0e0e0'>
	<tr bgcolor='#FFFFFF'>
		<td bgcolor='#e0e0e0'><p align=center><b><font color='#000000' face='Arial,Verdana' size='2'> Picture </font></b></td>
		<td bgcolor='#e0e0e0'><p align=center><b><font color='#000000' face='Arial,Verdana' size='2'> Description </font></b></td>
		<td bgcolor='#e0e0e0'><p align=center><b><font color='#000000' face='Arial,Verdana' size='2'> Quantity </font></b></td>
		<td bgcolor='#e0e0e0'><p align=center><b><font color='#000000' face='Arial,Verdana' size='2'> Unit Price </font></b></td>
		<td bgcolor='#e0e0e0'><p align=center><b><font color='#000000' face='Arial,Verdana' size='2'>Total</font></b></td>
	</tr>
	
	<tr bgcolor='#FFFFFF'>
		<td align="center"><img src="images/whitepolo.jpg"/></td>
		<td align="left" width="50%"><font face='verdana,arial' size=2>White Polo Shirt<br/>White men's polo shirt with embroidered logo.</td>
		<td width='10%' align=center>1</td>
		<td align="right" width="10%"><font face='verdana,arial' size=2>$1.99</font></td>
		<td align="right" width="10%"><font face='verdana,arial' size=2>$1.99</font></td>
	</tr>
	
	<tr bgcolor='#FFFFFF'>
		<td colspan="5" align="right"><img src="images\bmlpayfast.gif"/>&nbsp;&nbsp;&nbsp;&nbsp;<input type="submit" value="Checkout"/></td>
	</tr>

</form>