<% @Page Language="VB" Explicit="True" %>
<%'======================================================================================
'=  CardinalCommerce (http://www.cardinalcommerce.com)
'=  ccVoid.aspx
'=  Version 6.0 03/21/2006
'=
'=  Usage
'=		Sample page to execute a Void transaction.
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
<Title>Void Page</Title>
</Head>
<body>
<!--#Include  File = "ccMenu.aspx"-->
<br/>
<b>Void Request Form</b>
<form name="frm" method="POST" action="ccVoid.aspx?execute=true">
<TABLE>
<TR bgcolor="ffff40">
	<TD>TransactionId </TD><TD><input type=text size="60" name="transaction_id" value="<%=Session("TID")%>"></TD>
</TR>
<TR bgcolor="ffff40">
	<TD>Payment Type</TD>
	<TD><select name=txn_type>
		  <option value="C">C - Credit Card / Debit Card
  		  <option value="X">X - PayPal Express Checkout
	</select></TD>
</TR>
<TR>
	<TD>Description </TD><TD><input type=text size="60" name="description" value="Void Description"></TD>
</TR>
<TR>
  <TD></TD>
  <TD><input type=submit name="submit" value="Void"></TD>
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

		Dim strErrorNo, strErrorDesc, strStatus, strTransactionId, strReasonCode
		Dim centinelRequest As New CentinelRequest()
		Dim centinelResponse As New CentinelResponse()

		'======================================================================================
		' Construct Message using Name / Value pairs
		'======================================================================================

		centinelRequest.Add ("Version", Cstr(MessageVersion))
		centinelRequest.Add ("MsgType", "cmpi_void")
		centinelRequest.Add ("ProcessorId", Cstr(ProcessorId))
		centinelRequest.Add ("MerchantId", Cstr(MerchantId))
		centinelRequest.Add ("TransactionPwd", Cstr(TransactionPwd))
		centinelRequest.Add ("TransactionId", Cstr(request("transaction_id")))
		centinelRequest.Add ("TransactionType", Cstr(request("txn_type")))
		centinelRequest.Add ("Description", Cstr(request("description")))

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
			strStatus = centinelResponse.getValue("StatusCode")
			strTransactionId = centinelResponse.getValue("TransactionId")
			strReasonCode = centinelResponse.getValue("ReasonCode")

		End If
		
		centinelResponse = nothing
		centinelRequest = nothing
%>


<b> Void Transaction Request </b>
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
	<TD>Description</TD><TD><%=request("description")%></TD>
</TR>
</TABLE>
<br/><br/>
<b> Void Transaction Response </b>
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
	<TD>TransactionId</TD><TD><%=strTransactionId%></TD>
</TR>
</TABLE>


<%

	End IF
%>


</body>
</html>

