<% @Page Language="VB" Explicit="True" %>
<%'======================================================================================
'=  CardinalCommerce (http://www.cardinalcommerce.com)
'=  ccCapture.aspx
'=  Version 6.0.0 03/21/2006
'=
'=  Usage
'=		Sample page to execute a Capture transaction.
'=
'======================================================================================%>
<!-- #Include File="CentinelConfig.aspx"-->
<%@ import Namespace="CardinalCommerce" %>

<%

Session("TID") = request("transaction_id")
Session("amount") = request("amount")
Session("currencyCode") = request("currency_code")
Session("orderNumber") = request("order_number")

%>
<HTML>
<Head>
<Title>Capture Page</Title>
</Head>
<body>
<!--#Include  File = "ccMenu.aspx"-->
<br/>
<b>Capture Request Form</b>
<form name="frm" method="POST" action="ccCapture.aspx?execute=true">
<TABLE>
<TR bgcolor="ffff40">
	<TD>TransactionId </TD><TD><input type=text size="60" name="transaction_id" value="<%=Session("TID")%>"></TD>
</TR>
<TR bgcolor="ffff40">
	<TD>Payment Type</TD>
	<TD><select name=txn_type>
		  <option value="C">C - Credit Card / Debit Card
  		  <option value="X">X - PayPal Express Checkout
		  <option value="B">B - Bill Me Later
	</select></TD>
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
		  <option value="036">036 - AUD</option>
        </select>
    </TD>
</TR>
<TR bgcolor="ffff40">
	<TD>Capture Type </TD>
    <TD><select name="capture_type">
          <option value="Full">Full</option>
          <option value="Partial">Partial</option>
        </select>
    </TD>
</TR>
<TR>
	<TD>Desecription </TD><TD><input type=text size="60" name="description" value="Capture Description"></TD>
</TR>
<TR>
  <TD></TD>
  <TD><input type=submit name="submit" value="Process Capture"></TD>
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

		Dim strErrorNo, strErrorDesc, strStatus, strAVSResult, strCardCodeResult, strTransactionId, strReasonCode
		Dim centinelRequest As New CentinelRequest()
		Dim centinelResponse As New CentinelResponse()

		'======================================================================================
		' Construct Message using Name / Value pairs
		'======================================================================================

		centinelRequest.Add ("Version", Cstr(MessageVersion))
		centinelRequest.Add ("MsgType", "cmpi_capture")
		centinelRequest.Add ("ProcessorId", Cstr(ProcessorId))
		centinelRequest.Add ("MerchantId", Cstr(MerchantId))
		centinelRequest.Add ("TransactionPwd", Cstr(TransactionPwd))
		centinelRequest.Add ("TransactionId", Cstr(request("transaction_id")))
		centinelRequest.Add ("TransactionType", Cstr(request("txn_type")))
		centinelRequest.Add ("CaptureType", Cstr(request("capture_type")))
		centinelRequest.Add ("Amount", Cstr(request("amount")))
		centinelRequest.Add ("CurrencyCode", Cstr(request("currency_code")))
		centinelRequest.Add ("Description", Cstr(request("description")))

		Try
			centinelResponse = centinelRequest.sendHTTP(Cstr(TransactionURL), Cstr(Timeout))
		
		Catch ex As Exception
           
			strErrorNo = "9040"
			strErrorDesc = "Communication Error"
		End Try

		If strErrorNo = "" Then
				
			strErrorNo = centinelResponse.getValue("ErrorNo")			 
			strErrorDesc = centinelResponse.getValue("ErrorDesc")
			strStatus = centinelResponse.getValue("StatusCode")
			strAVSResult = centinelResponse.getValue("AVSResult")
			strCardCodeResult = centinelResponse.getValue("CardCodeResult")
			strTransactionId = centinelResponse.getValue("TransactionId")
			strReasonCode = centinelResponse.getValue("ReasonCode")

		End If
		
		centinelResponse = nothing
		centinelRequest = nothing
%>


<b> Capture Transaction Request</b>
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
	<TD>Capture Type</TD><TD><%=request("capture_type")%></TD>
</TR>
<TR>
	<TD>Amount</TD><TD><%=request("amount")%></TD>
</TR>
<TR>
	<TD>Currency Code</TD><TD><%=request("currency_code")%></TD>
</TR>
<TR>
	<TD>Memo</TD><TD><%=request("description")%></TD>
</TR>
</TABLE>
<br/><br/>
<b> Capture Transaction Response</b>
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
	<TD>AVS Result</TD><TD><%=strAVSResult%></TD>
</TR>
<TR>
	<TD>Card Code Result</TD><TD><%=strCardCodeResult%></TD>
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

