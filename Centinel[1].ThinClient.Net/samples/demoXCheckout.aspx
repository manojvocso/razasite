<% @Page Language="VB" Explicit="True"%>
<%response.Expires=-1%>
<%response.Buffer=true%>
<%@ import Namespace="CardinalCommerce" %>
<!-- #Include File="CentinelConfig.aspx"-->
<%
'========================================================================================
'=  Cardinal Commerce (http://www.cardinalcommerce.com)
'=  demoXCheckout.aspx
'=  Version 6.0
'=	03/01/2006
'=
'=  This page sends the cmpi_lookup message request to Centinel and processes the response
'=  for paypal express checkout transactions.
'=
'========================================================================================
%>
<HTML>
<Head>
<Title>Demo - PayPal Express Checkout</Title>

<%
	Dim strCardEnrolled, strErrorNo, strErrorDesc, strTransactionId, strECI, transactionType, transactionAmount, orderNumber
	Dim centinelRequest As New CentinelRequest()
	Dim centinelResponse As New CentinelResponse()
		
	transactionType = "X"
	transactionAmount = Session("Centinel_TransactionAmount")
	orderNumber = Session("Centinel_OrderNumber")
	Session("Centinel_TransactionType") = transactionType
	Session("Centinel_PaymentType") = "paypal_express"
	Session("Centinel_TermURL") = Class1.DemoTermURL

	'======================================================================================
	' Use the thin client Add method to construct the cmpi_lookup message. 
	' Additional API documentation is available within the Thin Client Documentation.
	'
	' Note that the MessageVersion, ProcessorId, MerchantId values are defined within the
	' CentinelConfig.aspx include file.
	'======================================================================================

	centinelRequest.Add ("Version", Cstr(MessageVersion))
	centinelRequest.Add ("MsgType", "cmpi_lookup")
	centinelRequest.Add ("ProcessorId", Cstr(ProcessorId))
	centinelRequest.Add ("MerchantId", Cstr(MerchantId))
	centinelRequest.Add ("TransactionPwd", Cstr(TransactionPwd))
	centinelRequest.Add ("TransactionType", Cstr(transactionType))
	centinelRequest.Add ("Amount", Cstr(transactionAmount))
	centinelRequest.Add ("CurrencyCode", "840")
	centinelRequest.Add ("OrderNumber", Cstr(orderNumber))
	centinelRequest.Add ("OrderDescription", "Order Descripton Goes Here")
	centinelRequest.Add ("UserAgent", Request.ServerVariables("HTTP_USER_AGENT"))
	centinelRequest.Add ("BrowserHeader", Request.ServerVariables("HTTP_ACCEPT"))
	centinelRequest.Add ("IPAddress", Request.ServerVariables("REMOTE_ADDR"))
	centinelRequest.Add ("EMail", "")
	centinelRequest.Add ("TransactionAction", "Authorization")
	centinelRequest.Add ("NoShipping", "N")
	centinelRequest.Add ("OverrideAddress", "N")
	centinelRequest.Add ("ForceAddress", "N")
	
	'=====================================================================================
	' Send the XML Msg to the MAPS Server
	' SendHTTP will send the cmpi_lookup message to the MAPS Server (requires fully qualified URL)
	' The Response is the CentinelResponse Object
	'=====================================================================================

	Try
		centinelResponse = centinelRequest.sendHTTP(Cstr(TransactionURL), Cstr(Timeout))

	Catch ex As Exception

		Session("Message") = "Your transaction was not able to complete, please choose another form of payment."
		Response.redirect("demoCheckout.aspx")

	End Try
		
	strCardEnrolled = centinelResponse.getValue("Enrolled")		
	strErrorNo = centinelResponse.getValue("ErrorNo")			 
	strErrorDesc = centinelResponse.getValue("ErrorDesc")
	strTransactionId = centinelResponse.getValue("TransactionId")
	strECI = centinelResponse.getValue("EciFlag")

	Session("Centinel_TransactionId") = strTransactionId
	Session("Centinel_Enrolled") = strCardEnrolled
	Session("Centinel_ErrorNo") = strErrorNo
	Session("Centinel_ErrorDesc") = strErrorDesc
	Session("Centinel_ECI") = strECI

	Session("Centinel_ACSURL") = centinelResponse.getValue("ACSUrl")
	Session("Centinel_PAYLOAD") = centinelResponse.getValue("Payload")	
	


	'=====================================================================================
	' Handle PayPal Express Checkout Logic
	'
	' If the Consumer is enrolled (CardEnrolled is Y) then the Consumer is redirected to
	' the page that transfers them to PayPal. 
	'
	' If the enrolled results were not successful (CardEnrolled is U) then the Consumer is
	' redirected to a page where they can select another form of payment. 
	'
	' Note that it is also important that you account for cases when your flow logic can account 
	' for error cases, and the flow can be broken after 'N' number of attempts
	'
	'=====================================================================================
	
		Session("CENTINEL_ISPAYPAL") = "Y"

		If strErrorNo = "0" AND strCardEnrolled = "Y" Then 

			Response.redirect ("demoTransfer.aspx")
		
		ElseIf strErrorNo = "0" AND strCardEnrolled = "U" Then

				'======================================================================================
				' Unable to process PayPal payment at this time.
				'======================================================================================

				Session("Message") = "PayPal express checkout payment was unable to complete. Please provide another form of payment to complete your transaction."
				Response.redirect ("demoCheckout.aspx")

		Else 
				
				'======================================================================================
				' Log Error Message, this is an unexpected state
				'======================================================================================

				Session("Message") = "An error occurred while processing the transaction. The transaction could not be completed."
				Response.redirect ("demoOrderComplete.aspx")

		End If


	centinelResponse = nothing
	centinelRequest = nothing
%>