<% @Page Language="VB" Explicit="True" debug="true"%>
<%response.Expires=-1%>
<%response.Buffer=true%>
<%@ import Namespace="CardinalCommerce" %>
<!-- #Include File="CentinelConfig.aspx"-->
<!-- #Include File="CentinelUtility.aspx"-->
<%
'========================================================================================
'=  Cardinal Commerce (http://www.cardinalcommerce.com)
'=  demoCheckoutProcessor.aspx
'=  Version 6.0
'=	03/01/2006
'=
'=  This page sends the cmpi_lookup request to Centinel and processes the response for Paypal,
'=  authentication, and SECURE-eBill transactions.  The page sends a cmpi_sale request to
'=  Centinel and processes the response for paypal direct payments.
'=
'========================================================================================
%>

<%

	Dim strCardEnrolled, strErrorNo, strErrorDesc, strTransactionId, strECI, transactionAmount, orderNumber
	Dim centinelRequest As New CentinelRequest()
	Dim centinelResponse As New CentinelResponse()

	Session("Centinel_TermURL") = Class1.DemoTermURL

	transactionAmount = Session("Centinel_TransactionAmount")
	orderNumber = Session("Centinel_OrderNumber")
	
	' Handles paypal direct payments
	If Session("Centinel_TransactionType") = "C" AND Session("Centinel_PaymentType") = "paypal_direct" Then
	
	
		Dim strTermUrl, strAcsUrl, strPAReq, strMerchantData, strTxnId, strAVSCode, strCvv2Code, strStatus, strReasonCode

		'======================================================================================
		' Use the thin client Add method to construct the cmpi_sale message. 
		' Additional API documentation is available within the Thin Client Documentation.
		'
		' Note that the MessageVersion, ProcessorId, MerchantId values are defined within the
		' CentinelConfig.aspx include file.
		'======================================================================================

		centinelRequest.Add ("Version", Cstr(MessageVersion))
		centinelRequest.Add ("MsgType", "cmpi_sale")
		centinelRequest.Add ("ProcessorId", Cstr(ProcessorId))
		centinelRequest.Add ("MerchantId", Cstr(MerchantId))
		centinelRequest.Add ("TransactionPwd", Cstr(TransactionPwd))
		centinelRequest.Add ("TransactionType", "C")
		centinelRequest.Add ("Amount", Cstr(transactionAmount))
		centinelRequest.Add ("CurrencyCode", "840")
		centinelRequest.Add ("CardNumber", request("cardNumberPP"))
		centinelRequest.Add ("CardExpMonth", request("expr_mmPP"))
		centinelRequest.Add ("CardExpYear", request("expr_yyyyPP"))
		centinelRequest.Add ("CardCode", request("cardCodePP"))
		centinelRequest.Add ("OrderNumber", Cstr(orderNumber))
		centinelRequest.Add ("OrderDescription", "This is where you put the order description")
		centinelRequest.Add ("IPAddress", Request.ServerVariables("REMOTE_ADDR"))
		centinelRequest.Add ("EMail", Session("Centinel_Email"))
		centinelRequest.Add ("BillingFirstName", Session("Centinel_BillingFirstName"))
		centinelRequest.Add ("BillingLastName", Session("Centinel_BillingLastName"))
		centinelRequest.Add ("BillingAddress1", Session("Centinel_BillingAddress"))
		centinelRequest.Add ("BillingAddress2", "")
		centinelRequest.Add ("BillingCity", Session("Centinel_BillingCity"))
		centinelRequest.Add ("BillingState", Session("Centinel_BillingState"))
		centinelRequest.Add ("BillingCountryCode", Session("Centinel_BillingCountrycode"))
		centinelRequest.Add ("BillingPostalCode", Session("Centinel_BillingPostalCode"))
		centinelRequest.Add ("ShippingFirstName", Session("Centinel_ShippingFirstName"))
		centinelRequest.Add ("ShippingLastName", Session("Centinel_ShippingLastName"))
		centinelRequest.Add ("ShippingAddress1", Session("Centinel_ShippingAddress"))
		centinelRequest.Add ("ShippingAddress2", "")
		centinelRequest.Add ("ShippingCity", Session("Centinel_ShippingCity"))
		centinelRequest.Add ("ShippingState", Session("Centinel_ShippingState"))
		centinelRequest.Add ("ShippingCountryCode", Session("Centinel_ShippingCountryCode"))
		centinelRequest.Add ("ShippingPostalCode", Session("Centinel_ShippingPostalCode"))

		' Send Message to the MAPS Server
		
		Try
		
			centinelResponse = centinelRequest.sendHTTP(Cstr(TransactionURL), Cstr(Timeout))

		Catch ex As Exception

			Session("Message") = "Your transaction was not able to complete, please choose another form of payment."
			Response.redirect("demoCheckout.aspx")

		End Try
		
		Session("Centinel_ErrorNo") = centinelResponse.getValue("ErrorNo")			 
		Session("Centinel_ErrorDesc") = centinelResponse.getValue("ErrorDesc")
		Session("Centinel_AVSResult") = centinelResponse.getValue("AVSResult")
		Session("Centinel_CardCodeResult") = centinelResponse.getValue("CardCodeResult")
		Session("Centinel_StatusCode") = centinelResponse.getValue("StatusCode")
		Session("Centinel_ReasonCode") = centinelResponse.getValue("ReasonCode")
		Session("Centinel_TransactionId") = centinelResponse.getValue("TransactionId")	
		
		'=====================================================================================
		' Handle PayPal Direct Payment Logic
		'
		' If the sale is successful (PAResStatus is Y) then the Consumer is redirected to
		' an order completion page. 
		'
		' If the is not successful (PAResStatus is N or X) then the Consumer is
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

			Session("Message") = "Payment was unable to complete. Please provide another form of payment to complete your transaction."
			Response.redirect ("demoCheckout.aspx")	

		End If
	
	' Handles PayPal Express Checkout Transactions
	ElseIf Session("Centinel_TransactionType") = "X" Then
	
		'======================================================================================
		' Use the thin client Add method to construct the cmpi_sale message. 
		' Additional API documentation is available within the Thin Client Documentation.
		'
		' Note that the MessageVersion, ProcessorId, MerchantId values are defined within the
		' CentinelConfig.aspx include file.
		'======================================================================================
	
		centinelRequest.Add ("Version", Cstr(MessageVersion))
		centinelRequest.Add ("MsgType", "cmpi_sale")
		centinelRequest.Add ("ProcessorId", Cstr(ProcessorId))
		centinelRequest.Add ("MerchantId", Cstr(MerchantId))
		centinelRequest.Add ("TransactionPwd", Cstr(TransactionPwd))
		centinelRequest.Add ("TransactionId", Session("Centinel_TransactionId"))
		centinelRequest.Add ("TransactionType", "X")
		centinelRequest.Add ("Amount", Cstr(transactionAmount))
		centinelRequest.Add ("CurrencyCode", "840")
		centinelRequest.Add ("OrderNumber", Cstr(orderNumber))
		centinelRequest.Add ("OrderDescription", "This is where you put the order description")
		centinelRequest.Add ("IPAddress", Request.ServerVariables("REMOTE_ADDR"))

		' Send Message to the MAPS Server
		
		Try
		
			centinelResponse = centinelRequest.sendHTTP(Cstr(TransactionURL), Cstr(Timeout))

		Catch ex As Exception

			Session("Message") = "Your transaction was not able to complete, please choose another form of payment."
			Response.redirect("demoCheckout.aspx")

		End Try
		
		Session("Centinel_ErrorNo") = centinelResponse.getValue("ErrorNo")			 
		Session("Centinel_ErrorDesc") = centinelResponse.getValue("ErrorDesc")
		Session("Centinel_AVSResult") = centinelResponse.getValue("AVSResult")
		Session("Centinel_CardCodeResult") = centinelResponse.getValue("CardCodeResult")
		Session("Centinel_StatusCode") = centinelResponse.getValue("StatusCode")
		Session("Centinel_ReasonCode") = centinelResponse.getValue("ReasonCode")
		Session("Centinel_TransactionId") = centinelResponse.getValue("TransactionId")		
	
		'=====================================================================================
		' Handle PayPal Express Checkout Logic
		'
		' If the sale is successful (PAResStatus is Y) then the Consumer is redirected to
		' an order completion page. 
		'
		' If the is not successful (PAResStatus is N or X) then the Consumer is
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

			Session("Message") = "PayPal express checkout payment was unable to complete. Please provide another form of payment to complete your transaction."
			Response.redirect ("demoCheckout.aspx")	

		End If	
	
	' Handles all other transactions
	Else
	
		'======================================================================================
		' Use the thin client Add method to construct the cmpi_lookup message. 
		' Additional API documentation is available within the Thin Client Documentation.
		'
		' Note that the MessageVersion, ProcessorId, MerchantId values are defined within the
		' CentinelConfig.aspx include file.
		'======================================================================================

		If Session("Centinel_TransactionType") = "P" OR Session("Centinel_TransactionType") = "E" Or Session("Centinel_TransactionType") = "B" Then
		
			centinelRequest.Add ("Version", Cstr(MessageVersion))
			centinelRequest.Add ("MsgType", "cmpi_lookup")
			centinelRequest.Add ("ProcessorId", Cstr(ProcessorId))
			centinelRequest.Add ("MerchantId", Cstr(MerchantId))
			centinelRequest.Add ("TransactionPwd", Cstr(TransactionPwd))
			centinelRequest.Add ("TransactionType", Session("Centinel_TransactionType"))
			centinelRequest.Add ("Amount", Session("Centinel_TransactionAmount"))
			centinelRequest.Add ("CurrencyCode", "840")
			centinelRequest.Add ("OrderNumber", Cstr(orderNumber))
			centinelRequest.Add ("OrderDescription", "This is where you put the order description")
			centinelRequest.Add ("UserAgent", Request.ServerVariables("HTTP_USER_AGENT"))
			centinelRequest.Add ("BrowserHeader", Request.ServerVariables("HTTP_ACCEPT"))
			centinelRequest.Add ("Installment", "")
			centinelRequest.Add ("IPAddress", Request.ServerVariables("REMOTE_ADDR"))
			centinelRequest.Add ("EMail", Session("Centinel_Email"))
			centinelRequest.Add ("BillingFirstName", Session("Centinel_BillingFirstName"))
			centinelRequest.Add ("BillingingMiddleName", "")
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
			
			If Session("Centinel_TransactionType") = "B" Then
				If request("promoCode") = "on" Then
					Session("Centinel_PromoCode") = "250Promo"
				Else
					Session("Centinel_PromoCode") = "Default"
				End If
				centinelRequest.Add ("PromoCode", Session("Centinel_PromoCode"))
			End If
		
		Else
		
			centinelRequest.Add ("Version", Cstr(MessageVersion))
			centinelRequest.Add ("MsgType", "cmpi_lookup")
			centinelRequest.Add ("ProcessorId", Cstr(ProcessorId))
			centinelRequest.Add ("MerchantId", Cstr(MerchantId))
			centinelRequest.Add ("TransactionPwd", Cstr(TransactionPwd))
			centinelRequest.Add ("TransactionType", Session("Centinel_TransactionType"))
			centinelRequest.Add ("Amount", Session("Centinel_TransactionAmount"))
			centinelRequest.Add ("CurrencyCode", "840")
			centinelRequest.Add ("CardNumber", request("cardNumber"))
			centinelRequest.Add ("CardExpMonth", request("expr_mm"))
			centinelRequest.Add ("CardExpYear", request("expr_yyyy"))
			centinelRequest.Add ("OrderNumber", Cstr(orderNumber))
			centinelRequest.Add ("OrderDescription", "This is where you put the order description")
			centinelRequest.Add ("UserAgent", Request.ServerVariables("HTTP_USER_AGENT"))
			centinelRequest.Add ("BrowserHeader", Request.ServerVariables("HTTP_ACCEPT"))
			centinelRequest.Add ("Installment", "")
			centinelRequest.Add ("IPAddress", Request.ServerVariables("REMOTE_ADDR"))
			centinelRequest.Add ("EMail", Session("Centinel_Email"))
			centinelRequest.Add ("BillingFirstName", Session("Centinel_BillingFirstName"))
			centinelRequest.Add ("BillingLastName", Session("Centinel_BillingLastName"))
			centinelRequest.Add ("BillingAddress1", Session("Centinel_BillingAddress"))
			centinelRequest.Add ("BillingAddress2", "")
			centinelRequest.Add ("BillingCity", Session("Centinel_BillingCity"))
			centinelRequest.Add ("BillingState", Session("Centinel_BillingState"))
			centinelRequest.Add ("BillingCountryCode", Session("Centinel_BillingCountrycode"))
			centinelRequest.Add ("BillingPostalCode", Session("Centinel_BillingPostalCode"))
			centinelRequest.Add ("ShippingFirstName", Session("Centinel_ShippingFirstName"))
			centinelRequest.Add ("ShippingLastName", Session("Centinel_ShippingLastName"))
			centinelRequest.Add ("ShippingAddress1", Session("Centinel_ShippingAddress"))
			centinelRequest.Add ("ShippingAddress2", "")
			centinelRequest.Add ("ShippingCity", Session("Centinel_ShippingCity"))
			centinelRequest.Add ("ShippingState", Session("Centinel_ShippingState"))
			centinelRequest.Add ("ShippingCountryCode", Session("Centinel_ShippingCountryCode"))
			centinelRequest.Add ("ShippingPostalCode", Session("Centinel_ShippingPostalCode"))

		End If
		
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

		If Session("Centinel_TransactionType") = "C" Then
			Session("Centinel_PIType") = determineCardType(request("cardNumber"))
		End IF

		'======================================================================================
		' Determine how to proceed with the transaction. Note that this example handles
		' PayPal payments, SECURE-eBill payments, and Payer Authentication processing.
		'======================================================================================

		'======================================================================================
		' Handle ALL PayPal Logic
		'======================================================================================

		If Session("Centinel_TransactionType") = "P"  Then
			
				Session("CENTINEL_ISPAYPAL") = "Y"

				If strErrorNo = "0" AND strCardEnrolled = "Y" Then 

					Response.redirect ("demoTransfer.aspx")

				ElseIf strErrorNo = "0" AND strCardEnrolled = "U" Then

						'======================================================================================
						' Unable to process PayPal payment at this time.
						'======================================================================================

						Session("Message") = "PayPal payment was unable to complete. Please provide another form of payment to complete your transaction."
						Response.redirect ("demoCheckout.aspx")

				Else 
						
						'======================================================================================
						' Log Error Message, this is an unexpected state
						'======================================================================================

						Session("Message") = "PayPal payment was unable to complete. Please provide another form of payment to complete your transaction."
						Response.redirect ("demoCheckout.aspx")

				End If
				
				
				
				
		ElseIf Session("Centinel_TransactionType") = "E" Then
		
		'======================================================================================
		' Handle SECURE-eBill Logic
		'======================================================================================

			If strErrorNo = "0" AND strCardEnrolled = "Y" Then 

				'======================================================================================
				' Proceed to SECURE-eBill Transaction
				'======================================================================================

				Response.redirect ("frames/ccFrame.aspx")

			ElseIf strErrorNo = "0" AND strCardEnrolled = "U" Then

				'======================================================================================
				' SECURE-eBill Payments are Not Available
				' Prompt for another form of payment
				'======================================================================================
					
				Session("Message") = "Secure-eBill Payment was unable to complete. Please provide another form of payment to complete your transaction."
				Response.redirect ("demoCheckout.aspx")
					

			Else 
						
				'==================================================================================
				' An error was encountered
				' Log Error Message, this is an unexpected state
				' Proceed to authorization to complete the transaction.
				'==================================================================================

				Session("Message") = "Secure-eBill Payment was unable to complete. Please provide another form of payment to complete your transaction."
				Response.redirect ("demoCheckout.aspx")

			End If				
				
				
		ElseIf Session("Centinel_TransactionType") = "B" Then
		'======================================================================================
		' Handle Bill Me Later Logic
		'======================================================================================

			If strErrorNo = "0" AND strCardEnrolled = "Y" Then 

				'======================================================================================
				' Proceed to Bill Me Later Transaction
				'======================================================================================

				Response.redirect ("frames/ccFrame.aspx")

			ElseIf strErrorNo = "0" AND strCardEnrolled = "U" Then

				'======================================================================================
				' Bill Me Later Payments are Not Available
				' Prompt for another form of payment
				'======================================================================================
					
				Session("Message") = "Bill Me Later Payment was unable to complete. Please provide another form of payment to complete your transaction."
				Response.redirect ("demoCheckout.aspx")

			Else 
						
				'==================================================================================
				' An error was encountered
				' Log Error Message, this is an unexpected state
				' Proceed to authorization to complete the transaction.
				'==================================================================================

				Session("Message") = "Bill Me Later Payment was unable to complete. Please provide another form of payment to complete your transaction."
				Response.redirect ("demoCheckout.aspx")

			End If
					

		Else

		'======================================================================================
		' Handle ALL Payer Authentication Logic
		'======================================================================================

			If strErrorNo = "0" AND strCardEnrolled = "Y" Then 

				'======================================================================================
				' Proceed to Payer Authentication
				'======================================================================================

				Response.redirect ("frames/ccFrame.aspx")

			ElseIf strErrorNo = "0" AND strCardEnrolled = "U" Then

				'======================================================================================
				' Proceed to Authorization, Payer Authentication Not Available
				' Set the proper ECI value based on the Card Type
				'======================================================================================

				Session("Message") = "Your Transaction has completed successfully."
				Response.redirect ("demoOrderComplete.aspx")


			ElseIf strErrorNo = "0" AND strCardEnrolled = "N" Then

				'======================================================================================
				' Proceed to Authorization, Payer Authentication Not Available
				' Set the proper ECI value based on the Card Type
				'======================================================================================

				Session("Message") = "Your Transaction has completed successfully."
				Response.redirect ("demoCompleteOrder.aspx")
						
			Else 
						
				'==================================================================================
				' An error was encountered
				' Log Error Message, this is an unexpected state
				' Proceed to authorization to complete the transaction.
				'==================================================================================

				Session("Message") = "The credit card payment was unable to complete. Please provide another form of payment to complete your transaction."
				Response.redirect ("demoCheckout.aspx")

			End If

		End If

	End If
	
	centinelResponse = nothing
	centinelRequest = nothing
%>
