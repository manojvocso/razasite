<% @Page Language="VB" Explicit="True" %>
<%response.Expires=-1%>
<%response.Buffer=true%>
<%
'======================================================================================
'=  CardinalCommerce (http://www.cardinalcommerce.com)
'=  ccLookup.aspx
'=  Version 6.0.0 03/21/2006
'=
'= Usage
'=	Sample Form to simulate the checkout process of a ecommerce website.
'=
'======================================================================================%>

<HTML>
<HEAD><Title>Centinel - Start Page</Title>
<script LANGUAGE="JavaScript">
function popUp(url) {
	popupWin=window.open(url,"win",'toolbar=0,location=0,directories=0,status=1,menubar=1,scrollbars=1,width=570,height=450');
	self.name = "mainWin"; }
</script>
</Head>
<BODY>
<!--#Include  File = "ccMenu.aspx"-->
<br/>
<b>Lookup Transaction Form</b>
<br/>
This form is intended to simulate your payment page within your ecommerce website.
<br/>All payment information is collected, and clicking the submit button at the bottom of the page simulates the consumer clicking the final buy button.
<br/>
<% If Len(Session("Message")) > 0 then %>
<br/>
	<font color="red"><b>Sample Message : <%=Session("Message")%></b></font>
<br/>
<% End If %>
<br/>
<Form name="frm" method=post action="ccProcessor.aspx">
<TABLE>
<TR>
	<td bgcolor="ffff40">Transaction Type</TD><TD>
	<select name="txn_type">
		  <option value="C">C - Credit Card / Debit Card
		  <option value="A">A - PayPal PreApproved Payments
		  <option value="P">P - PayPal
  		  <option value="X">X - PayPal Express Checkout
		  <option value="E">E - SECURE-eBill
  		  <option value="B">B - Bill-Me-Later
	</select></TD>
	<TD>Order Description </TD><TD><input type=text name="order_description" value="Sample Order..."></TD>
	<TD></TD><TD></TD>
</TR>
<TR><td>Card Number</TD><TD>
	<select name="card_number">
		  <option value="4000000000000002">4000000000000002 - Y,Y,Y
		  <option value="5200000000000007">5200000000000007 - Y,Y,Y
		  <option value="3000000000000004">3000000000000004 - Y,Y,Y
  		  <option value="1000000000000001">1000000000000001 - Error
		  <option value="4000000000000010">4000000000000010 - Y,Y,N
		  <option value="5200000000000015">5200000000000015 - Y,Y,N
		  <option value="3000000000000012">3000000000000012 - Y,Y,N
		  <option value="4000000000000028">4000000000000028 - Y,N,Y
		  <option value="3000000000000020">3000000000000020 - Y,N,Y
		  <option value="5200000000000023">5200000000000023 - Y,N,Y
		  <option value="4000000000000101">4000000000000101 - Y,A,Y
		  <option value="180000000000028">  180000000000028 - Y,A,Y
		  <option value="4000000000000036">4000000000000036 - Y,U
		  <option value="5200000000000031">5200000000000031 - Y,U
		  <option value="3000000000000038">3000000000000038 - Y,U
		  <option value="4000000000000044">4000000000000044 - Timeout Test
		  <option value="5200000000000049">5200000000000049 - Timeout Test
		  <option value="213100000000001">  213100000000001 - Timeout Test
		  <option value="4000000000000051">4000000000000051 - N
		  <option value="5200000000000056">5200000000000056 - N
		  <option value="213100000000019">  213100000000019 - N
		  <option value="4000000000000069">4000000000000069 - U
		  <option value="5200000000000064">5200000000000064 - U
		  <option value="213100000000027">  213100000000027 - U
		  <option value="4000000000000077">4000000000000077 - Error
		  <option value="5200000000000072">5200000000000072 - Error
		  <option value="213100000000035">  213100000000035 - Error
		  <option value="4000000000000085">4000000000000085 - Error 
		  <option value="5200000000000080">5200000000000080 - Error 
		  <option value="180000000000002">  180000000000002 - Error 
		  <option value="4000000000000093">4000000000000093 - Y, Error
		  <option value="5200000000000098">5200000000000098 - Y, Error
		  <option value="180000000000010">  180000000000010 - Y, Error
		  <option value="4000000400000093">4000000400000093 - Not Test Card 
		  <option value="5200000400000098">5200000400000098 - Not Test Card
		  <option value="180000040000010">  180000040000010 - Not Test Card
	</select></TD>
	<TD>Expiration </TD>
	<TD>
	<select name="expr_mm">
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
	<select name="expr_yyyy">
			<option value="2005">2005</option>
			<option value="2006">2006</option>
			<option value="2007">2007</option>
			<option value="2008" selected>2008</option>
			<option value="2009">2009</option>
	</select>
	</TD>
</TR>
<TR>
	<TD bgcolor="ffff40" width="150">Order Number</TD><TD><input type=text name="order_number" value="<%=randomOrderNumber%>"></TD>
	<TD>Transaction Action</TD><TD><input type=text name="payment_action" value="Authorization"></TD>
</TR>
<TR>
	<TD bgcolor="ffff40">Amount </TD><TD><input type=text name="amount" value="<%=randomAmount%>"></TD>
	<TD>No Shipping</TD><TD><input type=text name="no_shipping" value="N"></TD>
</TR>
<TR>
	<TD bgcolor="ffff40">Currency Code </TD><TD><input type=text name="currency_code" value="840"></TD>
	<TD>Force Address</TD><TD><input type=text name="force_address" value="N"></TD>
</TR>
<TR>
	<TD>Shipping Amount</TD><TD><input type=text name="shipping_amount" value="999"></TD>
	<TD>Tax Amount</TD><TD><input type=text name="tax_amount" value="399"></TD>
</TR>
<TR>
	<TD>Buyer Email</TD><TD><input type=text name="email_address" value="testuser@cardinalcommerce.com"></TD>
	<TD>Override Address</TD><TD><input type=text name="override_address" value="N"></TD>
</TR>
<TR>
	<TD width="150">Billing First Name</TD><TD><input type=text name="b_first_name" value="Test1"></TD>
	<TD width="150">Shipping First Name</TD><TD><input type=text name="s_first_name" value="Test1"></TD>
</TR>
<TR>
	<TD width="150">Billing Middle Name</TD><TD><input type=text name="b_middle_name" value=""></TD>
	<TD width="150">Shipping Middle Name</TD><TD><input type=text name="s_middle_name" value=""></TD>
</TR>
<TR>
	<TD>Billing Last Name</TD><TD><input type=text name="b_last_name" value="User"></TD>
	<TD>Shipping Last Name</TD><TD><input type=text name="s_last_name" value="User"></TD>
</TR>
<TR>
	<TD>Billing Address1</TD><TD><input type=text name="b_address1" value="1234 Main Street"></TD>
	<TD>Shipping Address1</TD><TD><input type=text name="s_address1" value="1234 Main Street"></TD>
</TR>
<TR>
	<TD>Billing Address2</TD><TD><input type=text name="b_address2" value=""></TD>
	<TD>Shipping Address2</TD><TD><input type=text name="s_address2" value=""></TD>
</TR>
<TR>
	<TD>Billing City</TD><TD><input type=text name="b_city" value="Mentor"></TD>
	<TD>Shipping City</TD><TD><input type=text name="s_city" value="Mentor"></TD>
</TR>
<TR>
	<TD>Billing State</TD><TD><input type=text name="b_state" value="OH"></TD>
	<TD>Shipping State</TD><TD><input type=text name="s_state" value="OH"></TD>
</TR>
<TR>
	<TD>Billing Postal Code</TD><TD><input type=text name="b_postal_code" value="44060"></TD>
	<TD>Shipping Postal Code</TD><TD><input type=text name="s_postal_code" value="44060"></TD>
</TR>
<TR>
	<TD>Billing Country Code</TD><TD><input type=text name="b_country_code" value="US"></TD>
	<TD>Shipping Country Code</TD><TD><input type=text name="s_country_code" value="US"></TD>
</TR>
<TR>
	<TD>Billing Phone</TD><TD><input type=text name="b_phone" value="4439211900"></TD>
	<TD>Shipping Phone</TD><TD><input type=text name="s_phone" value="4439211900"></TD>
</TR>
<TR>
	<TD></TD><TD></TD>
	<TD>Promo Code</TD><TD><input type=text name="promo_code" value="Default"></TD>
</TR>
<TR>
  <TD></TD>
  <TD><input type=submit name="submit" value="Submit Order"></TD>
</TR>
<TR>
  <TD colspan="2"><br><b><i>Required fields highlighted</i></b></TD>
</TR>
</TABLE>
</form>
</BODY>
</HTML> 