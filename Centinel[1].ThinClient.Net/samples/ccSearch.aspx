
<% @Page Language="VB" Explicit="True"%>
<%'======================================================================================
'=  CardinalCommerce (http://www.cardinalcommerce.com)
'=  ccSearch.aspx
'=  Version 6.0 03/21/2006
'=
'=  Usage
'=		Sample page to execute a transaction search.
'======================================================================================%>
<!-- #Include File="CentinelConfig.aspx"-->
<%@ import Namespace="CardinalCommerce" %>

<%

Session("TID") = request("transaction_id")
Session("reportFormat") = request("report_format")
Session("fromDt") = request("from_dt")
Session("toDt") = request("to_dt")
Session("payer") = request("payer")
Session("orderNumber") = request("order_number")
Session("rawAmount") = request("raw_amount")

dim today, yesterday, fromDt, toDt

today = DateTime.Now
yesterday = DateAdd("d",-1,DateTime.Now)

fromDt = Session("fromDt")
toDt = Session("toDt")

if len(fromDt) = 0 then
    fromDt = DatePart("m", yesterday) & "/" & DatePart("d", yesterday) & "/" & DatePart("yyyy", yesterday) & " 00:00:00"
end if
if len(toDt) = 0 then
    toDt = DatePart("m", today) & "/" & DatePart("d", today) & "/" & DatePart("yyyy", today) & " 00:00:00"
end if

%>
<HTML>
<Head>
<script LANGUAGE="JavaScript">
function clearResults() {
	document.frmResults.reportResults.value = ""
}
</script>
<Title>PayPal Transaction Search Page</Title>
</Head>
<Body>
<!--#Include  File = "ccMenu.aspx"-->
<br/>
<b>Transaction Search Request Form</b>
<form name="frm" method="POST" action="ccSearch.aspx?execute=true">
<TABLE>
<TR bgcolor="ffff40">
	<TD>From Date </TD><TD><input type=text name="from_dt" value="<%= fromDt %>"></TD>
</TR>
<TR bgcolor="ffff40">
	<TD>To Date </TD><TD><input type=text name="to_dt" value="<%= toDt %>"></TD>
</TR>
<TR bgcolor="ffff40">
	<TD>Report Format</TD>
    <TD><select name="report_format">
          <option value="CSV">CSV</option>
          <option value="XML">XML</option>
        </select>
    </TD>
</TR>
<TR>
	<TD>TransactionId </TD><TD><input type=text name="transaction_id" size="60" value="<%=Session("TID")%>"></TD>
</TR>
<TR>
	<TD>Payer </TD><TD><input type=text name="payer" size="60" value="<%=Session("payer")%>"></TD>
</TR>
<TR>
	<TD>Order Number </TD><TD><input type=text name="order_number" value="<%=Session("orderNumber")%>"></TD>
</TR>
<TR>
	<TD>Raw Amount </TD><TD><input type=text name="raw_amount" value="<%=Session("rawAmount")%>"></TD>
</TR>
<TR>
	<TD>Purchase Currency </TD>
    <TD><select name="purchase_currency">
          <option value="">Empty</option>
          <option value="840">840 - USD</option>
          <option value="978">978 - EUR</option>
          <option value="392">392 - JPY</option>
          <option value="124">124 - CAD</option>
          <option value="826">826 - GBP</option>
        </select>
    </TD>
</TR>
<TR>
	<TD>Transaction Type </TD>
    <TD><select name="transaction_type">
          <option value="">Empty</option>
          <option value="All">All</option>
          <option value="Sent">Sent</option>
          <option value="Received">Received</option>
          <option value="MassPay">MassPay</option>
          <option value="MoneyRequest">MoneyRequest</option>
          <option value="FundsAdded">FundsAdded</option>
          <option value="FundsWithdrawn">FundsWithdrawn</option>
          <option value="PayPalDebitCard">PayPalDebitCard</option>
          <option value="Referral">Referral</option>
          <option value="Fee">Fee</option>
          <option value="Subscription">Subscription</option>
          <option value="Dividend">Dividend</option>
          <option value="Billpay">Billpay</option>
          <option value="Refund">Refund</option>
          <option value="CurrencyConversions">CurrencyConversions</option>
          <option value="BalanceTransfer">BalanceTransfer</option>
          <option value="Reversal">Reversal</option>
          <option value="Shipping">Shipping</option>
          <option value="BalanceAffecting">BalanceAffecting</option>
          <option value="ECheck">ECheck</option>
          <option value="Invalid">Invalid</option>
        </select>
    </TD>
</TR>
<TR>
	<TD>Transaction Status </TD>
    <TD><select name="transaction_status">
          <option value="">Empty</option>
          <option value="Completed">Completed</option>
          <option value="Pending">Pending</option>
          <option value="Denied">Denied</option>
          <option value="Reversed">Reversed</option>
          <option value="Processing">Processing</option>
          <option value="Invalid">Invalid</option>
        </select>
    </TD>
</TR>
<TR>
  <TD></TD>
  <TD><input type=submit name="submit" value="Transaction Search" ></TD>
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


		Dim strErrorNo, strErrorDesc, strReportResults
		Dim centinelRequest As New CentinelRequest()
		Dim centinelResponse As New CentinelResponse()

		'======================================================================================
		' Construct Message using Name / Value pairs
		'======================================================================================


		centinelRequest.Add ("Version", Cstr(MessageVersion))
		centinelRequest.Add ("MsgType", "cmpi_paypal_transaction_search")
		centinelRequest.Add ("ProcessorId", Cstr(ProcessorId))
		centinelRequest.Add ("MerchantId", Cstr(MerchantId))
		centinelRequest.Add ("TransactionPwd", Cstr(TransactionPwd))
		centinelRequest.Add ("TransactionId", Cstr(request("transaction_id")))
		centinelRequest.Add ("FromDt", Cstr(request("from_dt")))
		centinelRequest.Add ("ToDt", Cstr(request("to_dt")))
		centinelRequest.Add ("ReportFormat", Cstr(request("report_format")))
		centinelRequest.Add ("Payer", Cstr(request("payer")))
		centinelRequest.Add ("Amount", Cstr(request("raw_amount")))
		centinelRequest.Add ("OrderNumber", Cstr(request("order_number")))
		centinelRequest.Add ("CurrencyCode", Cstr(request("purchase_currency")))
		centinelRequest.Add ("TransactionStatus", Cstr(request("transaction_status")))
		centinelRequest.Add ("TransactionType", Cstr(request("transaction_type")))


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
			strReportResults = centinelResponse.getValue("Report")
			
		End If
		
		centinelResponse = nothing
		centinelRequest = nothing
%>


<b> Transaction Search Request </b>
<TABLE width="300">
<TR>
	<TD>ProcessorId</TD><TD><%=Cstr(ProcessorId)%></TD>
</TR>
<TR>
	<TD>MerchantId</TD> <TD><%=Cstr(MerchantId)%></TD>
</TR>
<TR>
	<TD>TransactionPwd</TD><TD><%=Cstr(TransactionPwd)%></TD>
</TR>
<TR>
	<TD>TransactionId</TD><TD><%=request("transaction_id")%></TD>
</TR>
<TR>
	<TD>Report Format</TD><TD><%=request("report_format")%></TD>
</TR>
<TR>
	<TD>Payer </TD><TD><%=request("payer")%></TD>
</TR>
<TR>
	<TD>Order Number </TD><TD><%=request("order_number")%></TD>
</TR>
<TR>
	<TD>Amount </TD><TD><%=request("raw_amount")%></TD>
</TR>
<TR>
	<TD>Currency Code </TD><TD><%=request("purchase_currency")%></TD>
</TR>
<TR>
	<TD>Transaction Type </TD><TD><%=request("transaction_type")%></TD>
</TR>
<TR>
	<TD>Transaction Status </TD><TD><%=request("transaction_status")%></TD>
</TR>
<TR>
	<TD>&nbsp;</TD><TD>&nbsp;</TD>
</TR>
</table>
<br/><br/>
<b> Transaction Search Results </b>
<form name="frmResults">
<TABLE width="100%">
<TR>
	<TD>ErrorNo</TD><TD><%=strErrorNo%></TD>
</TR>
<TR>
	<TD>Description</TD><TD><%=strErrorDesc%></TD>
</TR>
<TR>
	<TD>Report</TD><TD><textarea name="reportResults" cols="100" rows="10"><%=strReportResults%></textarea></TD>
</TR>
</table>
</form>


<%

	End IF
%>


</body>
</html>

