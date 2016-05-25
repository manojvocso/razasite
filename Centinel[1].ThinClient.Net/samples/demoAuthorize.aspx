<% @Page Language="VB" Explicit="True" debug="true"%>
<%response.Expires=-1%>
<%response.Buffer=true%>
<%@ import Namespace="CardinalCommerce" %>
<!-- #Include File="CentinelConfig.aspx"-->
<%
'========================================================================================
'=  Cardinal Commerce (http://www.cardinalcommerce.com)
'=	demoAuthorize.aspx
'=  Version 6.0
'=	03/01/2006
'=
'=	This page is where the message is sent that executes the cmpi_authorize request after
'=  the customer has submitted the order.
'=
'========================================================================================
%>

<%

	Dim strErrorNo, strErrorDesc, strTransactionId, transactionAmount, orderNumber, strAuthCode, strAcctNum
	Dim oErrorNo, oErrorDesc, oAuthCode, oTransactionId, oAcctNum, oStatusCode, oReasonCode
	Dim regMonth, regDay, regYear, custRegDate
	Dim centinelRequest As New CentinelRequest()
	Dim centinelResponse As New CentinelResponse()
	

	Session("Centinel_TermURL") = Class1.DemoTermURL

	transactionAmount = Session("Centinel_TransactionAmount")
	orderNumber = Session("Centinel_OrderNumber")


	If Month(now) < 10 Then
		regMonth = "0" + CStr(Month(now))
	Else
		regMonth = CStr(Month(now))
	End If
	If Day(now) < 10 Then
		regDay = "0" + CStr(Day(now))
	Else
		regDay = CStr(Day(now))
	End If
	regYear = CStr(Year(now))
	custRegDate = regYear + regMonth + regDay


	' Handles Bill Me Later Transactions
	If Session("Centinel_TransactionType") = "B" Then
	
		'======================================================================================
		' Use the thin client Add method to construct the cmpi_authorize message. 
		' Additional API documentation is available within the Thin Client Documentation.
		'
		' Note that the MessageVersion, ProcessorId, MerchantId values are defined within the
		' CentinelConfig.aspx include file.
		'======================================================================================
	

		centinelRequest.Add ("Version", Cstr(MessageVersion))
		centinelRequest.Add ("MsgType", "cmpi_authorize")
		centinelRequest.Add ("ProcessorId", Cstr(ProcessorId))
		centinelRequest.Add ("MerchantId", Cstr(MerchantId))
		centinelRequest.Add ("TransactionPwd", Cstr(TransactionPwd))
		centinelRequest.Add ("TransactionId", Session("Centinel_TransactionId"))
		centinelRequest.Add ("TransactionType", "B")
		centinelRequest.Add ("Amount", Cstr(transactionAmount))
		centinelRequest.Add ("ShippingAmount", Cstr("000"))
		centinelRequest.Add ("CurrencyCode", "840")
		centinelRequest.Add ("OrderNumber", Cstr(orderNumber))
		centinelRequest.Add ("OrderDescription", "This is where you put the order description")
		centinelRequest.Add ("IPAddress", Request.ServerVariables("REMOTE_ADDR"))

		centinelRequest.Add ("PromoCode", Session("Centinel_PromoCode"))
		centinelRequest.Add ("CustomerRegistrationDate", Cstr(custRegDate))
		centinelRequest.Add ("CustomerFlag", Cstr("N"))
		centinelRequest.Add ("CategoryCode", Cstr("5400"))
		centinelRequest.Add ("TransactionMode", Cstr("S"))
		centinelRequest.Add ("ProductCode", Cstr("PHY"))
		centinelRequest.Add ("TermsAndConditions", Cstr("32103"))

		centinelRequest.Add ("EMail", Session("Centinel_Email"))
		centinelRequest.Add ("BillingFirstName", Session("Centinel_BillingFirstName"))
		centinelRequest.Add ("BillingMiddleName", "")
		centinelRequest.Add ("BillingLastName", Session("Centinel_BillingLastName"))
		centinelRequest.Add ("BillingAddress1", Session("Centinel_BillingAddress"))
		centinelRequest.Add ("BillingAddress2", "")
		centinelRequest.Add ("BillingCity", Session("Centinel_BillingCity"))
		centinelRequest.Add ("BillingState", Session("Centinel_BillingState"))
		centinelRequest.Add ("BillingCountryCode", Session("Centinel_BillingCountrycode"))
		centinelRequest.Add ("BillingPostalCode", Session("Centinel_BillingPostalCode"))
		centinelRequest.Add ("BillingPhone", Session("Centinel_BillingPhone"))
		centinelRequest.Add ("ShippingFirstName", Session("Centinel_ShippingFirstName"))
		centinelRequest.Add ("ShippingMiddleName", "")
		centinelRequest.Add ("ShippingLastName", Session("Centinel_ShippingLastName"))
		centinelRequest.Add ("ShippingAddress1", Session("Centinel_ShippingAddress"))
		centinelRequest.Add ("ShippingAddress2", "")
		centinelRequest.Add ("ShippingCity", Session("Centinel_ShippingCity"))
		centinelRequest.Add ("ShippingState", Session("Centinel_ShippingState"))
		centinelRequest.Add ("ShippingCountryCode", Session("Centinel_ShippingCountryCode"))
		centinelRequest.Add ("ShippingPostalCode", Session("Centinel_ShippingPostalCode"))
		centinelRequest.Add ("ShippingPhone", Session("Centinel_ShippingPhone"))


		' Send Message to the MAPS Server
		
		Try
		
			centinelResponse = centinelRequest.sendHTTP(Cstr(TransactionURL), Cstr(Timeout))

		Catch ex As Exception

			Session("Message") = "Your transaction was not able to complete, please choose another form of payment."
			Response.redirect("demoCheckout.aspx")

		End Try
			
	
		Session("Centinel_ErrorNo") = centinelResponse.getValue("ErrorNo") 
		Session("Centinel_ErrorDesc") = centinelResponse.getValue("ErrorDesc")
		Session("Centinel_TransactionId") = centinelResponse.getValue("TransactionId")
		Session("Centinel_AuthCode") = centinelResponse.getValue("AuthorizationCode")
		Session("Centinel_AcctNum") = centinelResponse.getValue("AccountNumber")	
		Session("Centinel_StatusCode") = centinelResponse.getValue("StatusCode")	
		Session("Centinel_ReasonCode") = centinelResponse.getValue("ReasonCode")
		
		
		'=====================================================================================
		' Handle Bill Me Later Logic
		'
		' If the Authorization is successful (StatusCode is Y) then the Consumer is redirected to
		' an order completion page. 
		'
		' If it is not successful (Status is N or E) then the Consumer is
		' redirected to a page where they can select another form of payment. 
		'
		' Note that it is also important that you account for cases when your flow logic can account 
		' for error cases, and the flow can be broken after 'N' number of attempts
		'
		'=====================================================================================

		If Session("Centinel_ErrorNo") = "0" AND Session("Centinel_StatusCode") = "Y" Then

			Session("Message") = "Your transaction completed successfully. Your order will be shipped immediately."
			Response.redirect ("demoOrderComplete.aspx")

		Else

			Session("Message") = "Bill Me Later<sup>&reg;</sup> was unable to authorize your transaction. Please select another payment method to complete your order."
			Response.redirect ("demoCheckout.aspx")			

		End If	

	Else

			Session("Message") = "Your transaction was unable to complete. Please select another payment method to complete your order."
			Response.redirect ("demoCheckout.aspx")			

	End If

	centinelResponse = nothing
	centinelRequest = nothing
%>
