
<% @Page Language="VB" Explicit="True" aspcompat="true" %>
<%'======================================================================================
'=  Cardinal Commerce (http://www.cardinalcommerce.com)
'=  ccAuthorize.aspx
'=  Version 6.0 03/21/2006
'=
'=  Usage
'=		Sample page to execute an authorization.
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
<Title>Authorize Page</Title>
</Head>
<body>
<!--#Include  File = "ccMenu.aspx"-->
<br/>
<b>Authorization Request Form</b>
<form name="frm" method="POST" action="ccAuthorize.aspx?execute=true">
<TABLE>
<TR>
	<td bgcolor="ffff40">Transaction Type</TD><TD>
	<select name=txn_type>
		  <option value="C">C - Credit Card / Debit Card
  		  <option value="X">X - PayPal Express Checkout
		  <option value="B">B - Bill-Me-Later
	</select></TD>
	<TD>Transaction Id</TD><TD><input type=text name="transaction_id" value=""></TD>
</TR>
<TR><td>Card Number</TD><TD>
	<input type=text name="PAN" value="">
	<!--<select name=PAN>
		  <option value="4000000000000002">4000000000000002 - VISA
		  <option value="5200000000000007">5200000000000007 - MC
  		  <option value="378282246310005">378282246310005  - AMEX
		  <option value="6011111111111117">6011111111111117 - DISC
		  <option value="5049900000000000">5049900000000000 - Bill-Me-Later
	</select>--></TD>
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
			<option value="2009">2009</option>
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
	<TD>Shipping Amount</TD><TD><input type=text name="shipping_amount" value="999"></TD>
	<TD>Tax Amount</TD><TD><input type=text name="tax_amount" value="399"></TD>
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
	<TD></TD><TD></TD>
</TR>
<TR>
	<TD width="150">Billing First Name</TD><TD><input type=text name="b_first_name" value="Joe"></TD>
	<TD width="150">Shipping First Name</TD><TD><input type=text name="s_first_name" value="Joe"></TD>
</TR>
<TR>
	<TD>Billing Middle Name</TD><TD><input type=text name="b_middle_name" value=""></TD>
	<TD>Shipping Middle Name</TD><TD><input type=text name="s_middle_name" value=""></TD>
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
	<TD>Customer Reg Date</TD><TD><input type=text name="c_reg_date" value="20060130"></TD>
	<TD>Promo Code</TD><TD><input type=text name="promo_code" value="Default"></TD>
</TR>
<TR>
	<TD>Customer Flag</TD><TD><input type=text name="c_new" value="N"></TD>
	<TD>Transaction Mode</TD><TD><input type=text name="txn_mode" value="S"></TD>
</TR>
<TR>
	<TD>Category Code</TD><TD><input type=text name="category_code" value="5400"></TD>
	<TD>Product Code</TD><TD><input type=text name="product_code" value="PHY"></TD>
</TR>
<TR>
	<TD>Has Checking Account</TD><TD><input type=text name="checking_account" value=""></TD>
	<TD>Has Savings Account</TD><TD><input type=text name="savings_account" value=""></TD>
</TR>
<TR>
	<TD>Household Income</TD><TD><input type=text name="household_income" value=""></TD>
	<TD>Household Income Currency</TD><TD><input type=text name="household_income_currency" value=""></TD>
</TR>
<TR>
	<TD>Years At Employer</TD><TD><input type=text name="years_employer" value=""></TD>
	<TD>Years At Residence</TD><TD><input type=text name="years_residence" value=""></TD>
</TR>
<TR>
	<TD>SSN</TD><TD><input type=text name="ssn" value=""></TD>
	<TD>DateOfBirth</TD><TD><input type=text name="dob" value=""></TD>
</TR>
<TR>
	<TD>Residence Status</TD><TD><input type=text name="residence_status" value=""></TD>
	<TD>Terms And Conditions</TD><TD><input type=text name="terms_conditions" value=""></TD>
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

		Dim strTermUrl, strCardEnrolled, strErrorNo, strErrorDesc, strAVSCode, strCardCode, strReasonCode
		Dim strStatus, strTransactionId, strAcsUrl, strPAReq, strMerchantData, strTxnId
		Dim strAccountNumber, strAuthCode

		Dim centinelRequest As New CentinelRequest()
		Dim centinelResponse As New CentinelResponse()

		'======================================================================================
		' Construct Message using Name / Value pairs
		'======================================================================================

		centinelRequest.Add ("Version", Cstr(MessageVersion))
		centinelRequest.Add ("MsgType", "cmpi_authorize")
		centinelRequest.Add ("ProcessorId", Cstr(ProcessorId))
		centinelRequest.Add ("MerchantId", Cstr(MerchantId))
		centinelRequest.Add ("TransactionPwd", Cstr(TransactionPwd))
		centinelRequest.Add ("TransactionId", Cstr(request("transaction_id")))
		centinelRequest.Add ("TransactionType", Cstr(request("txn_type")))
		centinelRequest.Add ("Amount", Cstr(request("amount")))
		centinelRequest.Add ("ShippingAmount", Cstr(request("shipping_amount")))
		centinelRequest.Add ("TaxAmount", Cstr(request("tax_amount")))
		centinelRequest.Add ("CurrencyCode", Cstr(request("currency_code")))
		centinelRequest.Add ("CardNumber", Cstr(request("PAN")))
		centinelRequest.Add ("CardExpMonth", Cstr(request("month_exp")))
		centinelRequest.Add ("CardExpYear", Cstr(request("year_exp")))
		centinelRequest.Add ("CardCode", Cstr(request("card_code")))
		centinelRequest.Add ("OrderNumber", Cstr(request("order_number")))
		centinelRequest.Add ("OrderDescription", Cstr(request("order_description")))
		centinelRequest.Add ("IPAddress", Cstr(Request.ServerVariables("REMOTE_ADDR")))
		centinelRequest.Add ("EMail", Cstr(request("email_address")))
		centinelRequest.Add ("BillingFirstName", Cstr(request("b_first_name")))
		centinelRequest.Add ("BillingMiddleName", Cstr(request("b_middle_name")))
		centinelRequest.Add ("BillingLastName", Cstr(request("b_last_name")))
		centinelRequest.Add ("BillingAddress1", Cstr(request("b_address1")))
		centinelRequest.Add ("BillingAddress2", Cstr(request("b_address2")))
		centinelRequest.Add ("BillingCity", Cstr(request("b_city")))
		centinelRequest.Add ("BillingState", Cstr(request("b_state")))
		centinelRequest.Add ("BillingCountryCode", Cstr(request("b_country_code")))
		centinelRequest.Add ("BillingPostalCode", Cstr(request("b_postal_code")))
		centinelRequest.Add ("BillingPhone", Cstr(request("b_phone")))
		centinelRequest.Add ("ShippingFirstName", Cstr(request("s_first_name")))
		centinelRequest.Add ("ShippingMiddleName", Cstr(request("s_middle_name")))
		centinelRequest.Add ("ShippingLastName", Cstr(request("s_last_name")))
		centinelRequest.Add ("ShippingAddress1", Cstr(request("s_address1")))
		centinelRequest.Add ("ShippingAddress2", Cstr(request("s_address2")))
		centinelRequest.Add ("ShippingCity", Cstr(request("s_city")))
		centinelRequest.Add ("ShippingState", Cstr(request("s_state")))
		centinelRequest.Add ("ShippingCountryCode", Cstr(request("s_country_code")))
		centinelRequest.Add ("ShippingPostalCode", Cstr(request("s_postal_code")))
		centinelRequest.Add ("ShippingPhone", Cstr(request("s_phone")))
		centinelRequest.Add ("PromoCode", Cstr(request("promo_code")))
		centinelRequest.Add ("CustomerRegistrationDate", Cstr(request("c_reg_date")))
		centinelRequest.Add ("CustomerFlag", Cstr(request("c_new")))
		centinelRequest.Add ("TransactionMode", Cstr(request("txn_mode")))
		centinelRequest.Add ("ProductCode", Cstr(request("product_code")))
		centinelRequest.Add ("CategoryCode", Cstr(request("category_code")))

		centinelRequest.Add ("HasCheckingAccount", Cstr(request("checking_account")))
		centinelRequest.Add ("HasSavingsAccount", Cstr(request("savings_account")))
		centinelRequest.Add ("ResidenceStatus", Cstr(request("residence_status")))
		centinelRequest.Add ("DateOfBirth", Cstr(request("dob")))
		centinelRequest.Add ("SSN", Cstr(request("ssn")))
		centinelRequest.Add ("TermsAndConditions", Cstr(request("terms_conditions")))
		centinelRequest.Add ("HouseholdIncome", Cstr(request("household_income")))
		centinelRequest.Add ("HouseholdIncomeCurrencyCode", Cstr(request("household_income_currency")))
		centinelRequest.Add ("YearsAtEmployer", Cstr(request("years_employer")))
		centinelRequest.Add ("YearsAtResidence", Cstr(request("years_residence")))

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
			strCardCode = centinelResponse.getValue("CardCodeResult")
			strReasonCode = centinelResponse.getValue("ReasonCode")
			strStatus = centinelResponse.getValue("StatusCode")
			strTransactionId = centinelResponse.getValue("TransactionId")
			strAccountNumber = centinelResponse.getValue("AccountNumber")
			strAuthCode = centinelResponse.getValue("AuthorizationCode")



		End If
		
		centinelResponse = nothing
		centinelRequest = nothing
%>


<b> Authorize Transaction Request</b>
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
<b> Authorize Transaction Response</b>
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
	<TD>Reason Code</TD><TD><%=strReasonCode%></TD>
</TR>
<TR>
	<TD>AVS Code Result</TD><TD><%=strAVSCode%></TD>
</TR>
<TR>
	<TD>Card Code Result</TD><TD><%=strCardCode%></TD>
</TR>
<TR>
	<TD>Auth TransactionId</TD><TD><%=strTransactionId%></TD>
</TR>
<TR>
	<TD>Account Number</TD><TD><%=strAccountNumber%></TD>
</TR>
<TR>
	<TD>Authorization Code</TD><TD><%=strAuthCode%></TD>
</TR>
</TABLE>


<%

	End IF
%>


</body>
</html>

