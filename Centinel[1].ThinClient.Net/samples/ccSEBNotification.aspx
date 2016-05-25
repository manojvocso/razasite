<% @Page Language="VB" Explicit="True" %>
<%
'=====================================================================================
'= CardinalCommerce (http://www.cardinalcommerce.com)
'= ccSEBNotification.aspx
'= Version 6.0 08/21/2006
'=
'= Usage
'=		Page handles SECURE-eBill Notification POSTs from Centinel
'=		The Notification POST alerts the merchant to a change in transaction
'=		status. This receives that POST and processes a status message
'=		to retrieve the transaction results.
'=
'=		Merchants should add functionality to handle SEB payment status
'=		changes in the locations marked "'// ADD LOGIC" below.
'=====================================================================================
%>
<!-- #Include File="CentinelConfig.aspx"-->
<%@ import Namespace="CardinalCommerce" %>

<%
    dim notificationId, reqProcessorId, reqMerchantId, strErrorNo, strErrorDesc
    dim errorNo, errorDesc, transactionId, status, signatureStatus, rawAmount, isoCurrencyNumber, orderNumber, orderDesc 
    dim personFirstName, personLastName, address1, address2, city, stateProvince, postalCode, countryCode, email
    dim paymentAmount, paymentDate, accountNumber, referenceId


    notificationId = Request("NotificationId")
    reqProcessorId = Request("ProcessorId")
    reqMerchantId = Request("MerchantId")

	' IIS Logging
	' Response.AppendToLog "SEB Notification Received: " & notificationId

    if len(notificationId) > 0 then

		Dim centinelRequest As New CentinelRequest()
		Dim centinelResponse As New CentinelResponse()

		'======================================================================================
		' Construct Message using Name / Value pairs
		'======================================================================================

		centinelRequest.Add ("MsgType", "cmpi_seb_status")
		centinelRequest.Add ("Version", Cstr(MessageVersion))
		centinelRequest.Add ("ProcessorId", Cstr(ProcessorId))
		centinelRequest.Add ("MerchantId", Cstr(MerchantId))
		centinelRequest.Add ("TransactionPwd", Cstr(TransactionPwd))
		centinelRequest.Add ("NotificationId", Cstr(notificationId))

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
    	
			' IIS Logging
			Response.AppendToLog ("cmpi_seb_status response: " & centinelResponse.GetUnparsedResponse())

			
			errorNo = centinelResponse.getValue("ErrorNo")			 
			errorDesc = centinelResponse.getValue("ErrorDesc")
			transactionId = centinelResponse.getValue("TransactionId")
			status = centinelResponse.getValue("TransactionStatus")
			signatureStatus = centinelResponse.getValue("SignatureVerification")
			rawAmount = centinelResponse.getValue("Amount")
			isoCurrencyNumber = centinelResponse.getValue("CurrencyCode")
			orderNumber = centinelResponse.getValue("OrderNumber")
			orderDesc = centinelResponse.getValue("OrderDescription")
			personFirstName = centinelResponse.getValue("FirstName")
			personLastName = centinelResponse.getValue("LastName")
			address1 = centinelResponse.getValue("Address1")
			address2 = centinelResponse.getValue("Address2")
			city = centinelResponse.getValue("City")
			stateProvince = centinelResponse.getValue("State")
			postalCode = centinelResponse.getValue("PostalCode")
			countryCode = centinelResponse.getValue("CountryCode")
			email = centinelResponse.getValue("EMail")
			paymentAmount = centinelResponse.getValue("PaymentAmount")
			paymentDate = centinelResponse.getValue("PaymentDate")
			accountNumber = centinelResponse.getValue("AccountNumber")
			referenceId = centinelResponse.getValue("ReferenceId")
	
		End If

		centinelResponse = nothing
		centinelRequest = nothing

        ' With the information provided, update the status of the order in
        ' your system.

        if status = "Pending" then

            '// ADD LOGIC
            '// - mark the order as pending in your system

        elseif status = "Refunded" then

            '// ADD LOGIC
            '// - mark the order as refunded in your system

        elseif status = "Paid" or status = "Partially Paid" then

            '// ADD LOGIC
            '// - mark the order as Paid in your system

        elseif status = "Completed" then

            '// ADD LOGIC

	elseif status = "Error" then

            '// ADD LOGIC

	elseif status = "Unexpected Payment" then

            '// ADD LOGIC

	elseif status = "Unrecognized Payment" then

            '// ADD LOGIC

	elseif status = "Redirected" then

            '// ADD LOGIC

        else
            '// ADD LOGIC
            '// - log an error, this should not be received

        end if

    else
		
	' IIS Logging
        ' Response.AppendToLog "Centinel Notification POST Error MISSING PARAMETER notification_id"

    end if
%>
