<% @Page Language="VB" Explicit="True"%>
<%'======================================================================================
'=  CardinalCommerce (http://www.cardinalcommerce.com)
'=  ccSale.aspx
'=  Version 6.0 03/21/2006
'=
'=  Usage
'=		Sample page to execute a Sale transaction.
'======================================================================================%>
<!-- #Include File="CentinelConfig.aspx"-->
<%@ import Namespace="CardinalCommerce" %>

<%

Session("TID") = request("transaction_id")
Session("authAmount") = request("auth_amount")
Session("currencyCode") = request("currency_code")

%>
<HTML>
<Head>
<Title>Sale Page</Title>
</Head>
<body>
<!--#Include  File = "ccMenu.aspx"-->
<br/>
<b>Sale Request Form</b>
<form name="frm" method="POST" action="ccSale.aspx?execute=true">
<TABLE>
<TR>
	<td bgcolor="ffff40">Transaction Type</TD><TD>
	<select name=txn_type>
		  <option value="C">C - Credit Card / Debit Card
  		  <option value="X">X - PayPal Express Checkout
	</select></TD>
	<TD>Transaction Id</TD><TD><input type=text name="transaction_id" value=""></TD>
</TR>
<TR><td>Card Number</TD><TD>
	<select name=PAN>
		  <option value="4000000000000002">4000000000000002 - VISA
		  <option value="5200000000000007">5200000000000007 - MC
		  <option value="3000000000000004">3000000000000004 - JCB
  		  <option value="378282246310005 ">378282246310005  - AMEX
		  <option value="6011111111111117">6011111111111117 - DISC
	</select></TD>
	<TD>Expiration </TD>
	<TD>
	<select name="month_exp">
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
	<select name="year_exp">
			<option value="2005">2005</option>
			<option value="2006">2006</option>
			<option value="2007">2007</option>
			<option value="2008" selected>2008</option>
			<option value="2015">2015</option>
	</select>
	</TD>
</TR>
<TR>
	<TD bgcolor="ffff40" width="150">Order Number</TD><TD><input type=text name="order_number" value="<%=randomOrderNumber%>"></TD>
	<TD>Order Description </TD><TD><input type=text name="order_description" value="Sample Order..."></TD>
</TR>
<TR>
	<TD bgcolor="ffff40">Amount </TD><TD><input type=text name="amount" value="<%=randomAmount%>"></TD>
	<TD>Recurring Freq </TD><TD><input type=text name="RecurringFrequency" value="28"></TD>
</TR>
<TR>
	<TD bgcolor="ffff40">Currency </TD><TD><input type=text name="currency_code" value="840"></TD>
	<TD>Recurring End </TD><TD><input type=text name="RecurringEnd" value="20030808"></TD>
</TR>
<TR>
	<TD>Buyer Email</TD><TD><input type=text name="email_address" value="testuser@cardinalcommerce.com"></TD>
	<TD>Installments</TD><TD><input type=text name="Installment" value=""></TD>
</TR>
<TR>
	<TD>Card Code</TD><TD><input type=text name="card_code" value="123"></TD>
	<TD width="150">Recurring </TD><TD>
					<select name="Recurring">
						<option value="Y" >Y
						<option value="N" selected>N
					</select></TD>
	
</TR>
<TR>
	<TD width="150">Billing First Name</TD><TD><input type=text name="b_first_name" value="Joe"></TD>
	<TD width="150">Shipping First Name</TD><TD><input type=text name="s_first_name" value="Joe"></TD>
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
  <TD></TD>
  <TD><input type=submit name="submit" value="Submit Order"></TD>
</TR>
<TR>
  <TD colspan="2"><br><b><i>Required fields highlighted</i></b></TD>
</TR>
</table>
</form>
<br>
<hr>
<br><br>
<%

	Dim executeTransaction
	executeTransaction = request("execute")

	if (executeTransaction = "true") then

		Dim strErrorNo, strErrorDesc, strAVSCode, strCvv2Code, strStatus, strReasonCode, strTransactionId
		Dim centinelRequest As New CentinelRequest()
		Dim centinelResponse As New CentinelResponse()

		'======================================================================================
		' Construct Message using Name / Value pairs
		'======================================================================================

		centinelRequest.Add ("Version", Cstr(MessageVersion))
		centinelRequest.Add ("MsgType", "cmpi_sale")
		centinelRequest.Add ("ProcessorId", Cstr(ProcessorId))
		centinelRequest.Add ("MerchantId", Cstr(MerchantId))
		centinelRequest.Add ("TransactionPwd", Cstr(TransactionPwd))
		centinelRequest.Add ("TransactionId", Cstr(request("transaction_id")))
		centinelRequest.Add ("TransactionType", Cstr(request("txn_type")))
		centinelRequest.Add ("Amount", Cstr(request("amount")))
		centinelRequest.Add ("CurrencyCode", Cstr(request("currency_code")))
        'centinelRequest.Add ("CardNumber", Cstr(request("PAN")))
        '	centinelRequest.Add ("CardExpMonth", Cstr(request("month_exp")))
        '	centinelRequest.Add ("CardExpYear", Cstr(request("year_exp")))
        '   centinelRequest.add("CardCode", CStr(Request("card_code")))
		centinelRequest.Add ("OrderNumber", Cstr(request("order_number")))
		centinelRequest.Add ("OrderDescription", Cstr(request("order_description")))
		centinelRequest.Add ("IPAddress", Cstr(Request.ServerVariables("REMOTE_ADDR")))
		centinelRequest.Add ("EMail", Cstr(request("email_address")))
		centinelRequest.Add ("BillingFirstName", Cstr(request("b_first_name")))
		centinelRequest.Add ("BillingLastName", Cstr(request("b_last_name")))
		centinelRequest.Add ("BillingAddress1", Cstr(request("b_address1")))
		centinelRequest.Add ("BillingAddress2", Cstr(request("b_address2")))
		centinelRequest.Add ("BillingCity", Cstr(request("b_city")))
		centinelRequest.Add ("BillingState", Cstr(request("b_state")))
		centinelRequest.Add ("BillingCountryCode", Cstr(request("b_country_code")))
		centinelRequest.Add ("BillingPostalCode", Cstr(request("b_postal_code")))
		centinelRequest.Add ("ShippingFirstName", Cstr(request("s_first_name")))
		centinelRequest.Add ("ShippingLastName", Cstr(request("s_last_name")))
		centinelRequest.Add ("ShippingAddress1", Cstr(request("s_address1")))
		centinelRequest.Add ("ShippingAddress2", Cstr(request("s_address2")))
		centinelRequest.Add ("ShippingCity", Cstr(request("s_city")))
		centinelRequest.Add ("ShippingState", Cstr(request("s_state")))
		centinelRequest.Add ("ShippingCountryCode", Cstr(request("s_country_code")))
		centinelRequest.Add ("ShippingPostalCode", Cstr(request("s_postal_code")))

		'=====================================================================================
		' Send the XML Msg to the MAPS Server
		' SendHTTP will send the cmpi_lookup message to the MAPS Server (requires fully qualified URL)
		' The Response is the CentinelResponse Object
		'=====================================================================================
		
		Try
			centinelResponse = centinelRequest.sendHTTP(Cstr(TransactionURL), Cstr(Timeout))
		
		Catch ex As Exception
           
			strErrorNo = "9040"
			strErrorDesc = "Communication Error"
		End Try

		If strErrorNo = "" Then
			
			strErrorNo = centinelResponse.getValue("ErrorNo")			 
			strErrorDesc = centinelResponse.getValue("ErrorDesc")
			strAVSCode = centinelResponse.getValue("AVSResult")
			strCvv2Code = centinelResponse.getValue("CardCodeResult")
			strStatus = centinelResponse.getValue("StatusCode")
			strReasonCode = centinelResponse.getValue("ReasonCode")
			strTransactionId = centinelResponse.getValue("TransactionId")
			
		End If
			
		centinelResponse = nothing
		centinelRequest = nothing
%>


<b> Sale Transaction Request</b>
<TABLE>
<TR>
	<TD>ProcessorId</TD>
	<TD><%=Cstr(ProcessorId)%></TD>
</TR>
<TR>
	<TD>MerchantId</TD>
	<TD><%=Cstr(MerchantId)%></TD>
</TR>
<TR>
	<TD>TransactionPwd</TD><TD><%=Cstr(TransactionPwd)%></TD>
</TR>
<TR>
	<TD>TransactionId</TD><TD><%=request("transaction_id")%></TD>
</TR>
<TR>
	<TD>Transaction Type</TD><TD><%=request("txn_type")%></TD>
</TR>
<TR>
	<TD>Amount</TD><TD><%=request("amount")%></TD>
</TR>
<TR>
	<TD>Currency Code</TD><TD><%=request("currency_code")%></TD>
</TR>
<TR>
	<TD>Description</TD><TD><%=request("order_description")%></TD>
</TR>
</TABLE>
<br/><br/>
<b> Sale Transaction Response</b>
<TABLE>
<TR>
	<TD>ErrorNo</TD><TD><%=strErrorNo%></TD>
</TR>
<TR>
	<TD>Description</TD><TD><%=strErrorDesc%></TD>
</TR>
<TR>
	<TD>Status Code</TD><TD><%=strStatus%></TD>
</TR>
<TR>
	<TD>AVS Code Result</TD><TD><%=strAVSCode%></TD>
</TR>
<TR>
	<TD>Card Code Result</TD><TD><%=strCvv2Code%></TD>
</TR>
<TR>
	<TD>Reason Code</TD><TD><%=strReasonCode%></TD>
</TR>
<TR>
	<TD>TransactionId</TD><TD><%=strTransactionId%></TD>
</TR>
</TABLE>


<%

	End IF
%>


</body>
</html>

