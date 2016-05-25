<% @Page Language="VB" Explicit="True"%>
<%'======================================================================================
'=  CardinalCommerce (http://www.cardinalcommerce.com)
'=  ccAuthBridge.aspx
'=  Version 6.0.0 03/21/2006
'=
'======================================================================================%>
<!-- #Include File="CentinelConfig.aspx"-->
<%@ import Namespace="CardinalCommerce" %>

<%

Session("TID") = request("transaction_id")
Session("refundAmount") = request("refund_amount")
Session("currencyCode") = request("currency_code")

%>
<HTML>
<Head>
<Title>Auth-Bridge Lookup Page</Title>
</Head>
<body>
<!--#Include  File = "ccMenu.aspx"-->
<br/>
<b>Auth-Bridge Lookup Request Form</b>
<form name="frm" method="POST" action="ccAuthBridge.aspx?execute=true">
<TABLE>
<TR bgcolor="ffff40">
	<TD>TransactionId </TD><TD><input type=text size="60" name="transaction_id" value="<%=Session("TID")%>"></TD>
</TR>
<TR bgcolor="ffff40">
	<td>Transaction Type</TD><TD>
	<select name=txn_type>
		  <option value="C">C - Credit Card / Debit Card
	</select></TD>
</TR>
<TR>
	<TD>Card Number </TD><TD><input type=text name="pan" value=""></TD>
</TR>
<TR>
	<TD>Amount </TD><TD><input type=text name="amount" value=""></TD>
</TR>
<TR>
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
<TR>
	<TD>From Date </TD><TD><input type=text size="10" name="from_dt" value=""></TD>
</TR>
<TR>
	<TD>To Date </TD><TD><input type=text size="10" name="to_dt" value=""></TD>
</TR>

<TR>
  <TD></TD>
  <TD><input type=submit name="submit" value="Process"></TD>
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

		Dim strErrorNo, strErrorDesc, strCAVV, strXID, strECI, strEnrolled, strStatus, strSignature
		Dim centinelRequest As New CentinelRequest()
		Dim centinelResponse As New CentinelResponse()

		'======================================================================================
		' Construct Message using Name / Value pairs
		'======================================================================================

		centinelRequest.Add ("Version", Cstr(MessageVersion))
		centinelRequest.Add ("MsgType", "cmpi_ab_lookup")
		centinelRequest.Add ("ProcessorId", Cstr(ProcessorId))
		centinelRequest.Add ("MerchantId", Cstr(MerchantId))
		centinelRequest.Add ("TransactionPwd", Cstr(TransactionPwd))
		centinelRequest.Add ("TransactionId", Cstr(request("transaction_id")))
		centinelRequest.Add ("TransactionType", Cstr(request("txn_type")))
		centinelRequest.Add ("CardNumber", Cstr(request("pan")))
		centinelRequest.Add ("Amount", Cstr(request("amount")))
		centinelRequest.Add ("CurrencyCode", Cstr(request("currency_code")))
		centinelRequest.Add ("FromDt", Cstr(request("from_dt")))
		centinelRequest.Add ("ToDt", Cstr(request("to_dt")))

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
			strCAVV = centinelResponse.getValue("Cavv")
			strXID = centinelResponse.getValue("Xid")
			strECI = centinelResponse.getValue("EciFlag")
			strEnrolled = centinelResponse.getValue("Enrolled")
			strStatus = centinelResponse.getValue("PAResStatus")
			strSignature = centinelResponse.getValue("SignatureVerification")

		End If
		
		centinelResponse = nothing
		centinelRequest = nothing
%>


<b> Auth-Bridge Transaction Request</b>
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
	<TD>From Date</TD><TD><%=request("from_dt")%></TD>
</TR>
<TR>
	<TD>To Date</TD><TD><%=request("to_dt")%></TD>
</TR>
</TABLE>
<br/><br/>
<b> Auth-Bridge Transaction Results </b>
<TABLE>
<TR>
	<TD>ErrorNo</TD><TD><%=strErrorNo%></TD>
</TR>
<TR>
	<TD>Description</TD><TD><%=strErrorDesc%></TD>
</TR>
<TR>
	<TD>CAVV</TD><TD><%=strCAVV%></TD>
</TR>
<TR>
	<TD>XID</TD><TD><%=strXID%></TD>
</TR>
<TR>
	<TD>ECI</TD><TD><%=strECI%></TD>
</TR>
<TR>
	<TD>Enrolled</TD><TD><%=strEnrolled%></TD>
</TR>
<TR>
	<TD>Authentication Status</TD><TD><%=strStatus%></TD>
</TR>
<TR>
	<TD>Signature Status</TD><TD><%=strSignature%></TD>
</TR>
</TABLE>


<%

	End IF
%>


</body>
</html>

