<% @Page Language="VB" Explicit="True" %>
<%'======================================================================================
'=  CardinalCommerce (http://www.cardinalcommerce.com)
'=  ccInitiatePayment.aspx
'=  Version 6.0.0 03/21/2006
'=
'=  Usage
'=	Sample page to execute a paypal merchant-initiated payment.
'======================================================================================%>
<!-- #Include File="CentinelConfig.aspx"-->
<%@ import Namespace="CardinalCommerce" %>

<%

Session("transactionId") = request("transaction_id")
Session("amount") = request("amount")
Session("currencyCode") = request("currency_code")

%>
<HTML>
<Head>
<Title>PayPal Preapproved Payment Page</Title>
</Head>
<Body>
<!--#Include  File = "ccMenu.aspx"-->
<br/>
<b>PreApproved Payment Request Form</b>
<form name="frm" method="POST" action="ccInitiatePayment.aspx?execute=true">
<TABLE>
<TR bgcolor="ffff40">
	<TD>Transaction Id<br><small>(of the agreement transaction)</small></TD><TD><input type=text size="60" name="transaction_id" value="<%=Session("transactionId")%>"></TD>
</TR>
<TR bgcolor="ffff40">
	<TD>Amount </TD><TD><input type=text name="amount" value="<%=Session("amount")%>"></TD>
</TR>
<TR bgcolor="ffff40">
	<TD>Currency Code </TD>
    <TD><select name="currency_code">
          <option value="840">840 - USD</option>
          <option value="978">978 - EUR</option>
          <option value="392">392 - JPY</option>
          <option value="124">124 - CAD</option>
          <option value="826">826 - GBP</option>
          <option value="826">036 - AUD</option>
        </select>
    </TD>
</TR>
<TR>
	<TD>Order Number </TD><TD><input type=text name="order_number" value="<%=randomOrderNumber%>"></TD>
</TR>
<TR>
	<TD>Description </TD><TD><input type=text size="60" name="description" value="PreApproved Description"></TD>
</TR>
<TR>
	<TD>Notification URL </TD><TD><input type=text size="60" name="notificationLocation" value=""></TD>
</TR>
<TR>
  <TD colspan="2"><font size="-1">Overrides the Merchant Configuration's PayPal Notification Location for this transaction.</font></TD>
</TR>
<TR>
  <TD></TD>
  <TD><input type=submit name="submit" value="Initiate"></TD>
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

		Dim strErrorNo, strErrorDesc, strTransactionId, strStatus, strStatusCode, strReasonCode
		Dim centinelRequest As New CentinelRequest()
		Dim centinelResponse As New CentinelResponse()


		'======================================================================================
		' Construct Message using Name / Value pairs
		'======================================================================================

		centinelRequest.Add ("Version", Cstr(MessageVersion))
		centinelRequest.Add ("MsgType", "cmpi_paypal_preapproved_payment")
		centinelRequest.Add ("ProcessorId", Cstr(ProcessorId))
		centinelRequest.Add ("MerchantId", Cstr(MerchantId))
		centinelRequest.Add ("TransactionPwd", Cstr(TransactionPwd))
		centinelRequest.Add ("TransactionId", Cstr(request("transaction_id")))
		centinelRequest.Add ("Amount", Cstr(request("amount")))
		centinelRequest.Add ("CurrencyCode", Cstr(request("currency_code")))
		centinelRequest.Add ("OrderNumber", Cstr(request("order_number")))
		centinelRequest.Add ("Description", Cstr(request("description")))
		centinelRequest.Add ("NotificationLocation", Cstr(request("notificationLocation")))

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
			strTransactionId = centinelResponse.getValue("TransactionId")
			strStatus = centinelResponse.getValue("PayPalStatus")
			strStatusCode = centinelResponse.getValue("StatusCode")
			strReasonCode = centinelResponse.getValue("ReasonCode")
		
		End If
		
		centinelResponse = nothing
		centinelRequest = nothing
%>


<b> PreApproved Payment Request Results </b>
<TABLE>
<TR>
	<TD>Processor Id</TD>
	<TD><%=Cstr(ProcessorId)%></TD>
</TR>
<TR>
	<TD>Merchant Id</TD>
	<TD><%=Cstr(MerchantId)%></TD>
</TR>
<TR>
	<TD>Transaction Pwd</TD><TD><%=Cstr(TransactionPwd)%></TD>
</TR>
<TR>
	<TD>Transaction Id<br><small>(of the agreement transaction)</small></TD><TD><%=request("transaction_id")%></TD>
</TR>
<TR>
	<TD>Amount</TD><TD><%=request("amount")%></TD>
</TR>
<TR>
	<TD>Currency Code</TD><TD><%=request("currency_code")%></TD>
</TR>
<TR>
	<TD>Order Number</TD><TD><%=request("order_number")%></TD>
</TR>
<TR>
	<TD>Description</TD><TD><%=request("description")%></TD>
</TR>
<TR>
	<TD>Notification URL</TD><TD><%=request("notificationLocation")%></TD>
</TR>
</TABLE>
<br/><br/>
<b> PreApproved Payment Results </b>
<TABLE>
<TR>
	<TD>ErrorNo</TD><TD><%=strErrorNo%></TD>
</TR>
<TR>
	<TD>Description</TD><TD><%=strErrorDesc%></TD>
</TR>
<TR>
	<TD>PayPal Status</TD><TD><%= strStatus %></TD>
</TR>
<TR>
	<TD>Status Code</TD><TD><%=strStatusCode%></TD>
</TR>
<TR>
	<TD>Reason Code</TD><TD><%=strReasonCode%></TD>
</TR>
<TR>
	<TD>Transaction Id</TD><TD><%= strTransactionId %></TD>
</TR>
</TABLE>


<%

	End IF
%>


</body>
</html>

