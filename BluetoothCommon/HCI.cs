﻿namespace NativeLayer.Bluetooth
{
    /// <summary>
    ///     (Bluetooth) Host Controller Interface (HCI)
    /// </summary>
    public static class HCI
    {
        public enum Command : ushort
        {
            HCI_Null = 0x0000,
            /// <summary>
            ///     Command to accept a new connection request.
            /// </summary>
            HCI_Accept_Connection_Request = 0x0409,
            /// <summary>
            ///     Command to reject a new connection request.
            /// </summary>
            HCI_Reject_Connection_Request = 0x040A,
            /// <summary>
            ///     Command to determine the user friendly name of the connected device.
            /// </summary>
            HCI_Remote_Name_Request = 0x0419,
            /// <summary>
            ///     Command to reset the host controller, link manager and the radio module.
            /// </summary>
            HCI_Reset = 0x0C03,
            /// <summary>
            ///     Command to set the status of the Scan_Enable configuration.
            /// </summary>
            HCI_Write_Scan_Enable = 0x0C1A,
            HCI_Read_Buffer_Size = 0x1005,
            HCI_Read_BD_ADDR = 0x1009,
            HCI_Read_Local_Version_Info = 0x1001,
            HCI_Create_Connection = 0x0405,
            HCI_Disconnect = 0x0406,
            HCI_Link_Key_Request_Reply = 0x040B,
            HCI_Link_Key_Request_Negative_Reply = 0x040C,
            HCI_PIN_Code_Request_Reply = 0x040D,
            HCI_PIN_Code_Request_Negative_Reply = 0x040E,
            HCI_Inquiry = 0x0401,
            HCI_Inquiry_Cancel = 0x0402,
            HCI_Write_Inquiry_Transmit_Power_Level = 0x0C59,
            HCI_Write_Inquiry_Mode = 0x0C45,
            HCI_Write_Simple_Pairing_Mode = 0x0C56,
            HCI_Write_Simple_Pairing_Debug_Mode = 0x1804,
            HCI_Write_Authentication_Enable = 0x0C20,
            HCI_Write_Page_Timeout = 0x0C18,
            HCI_Write_Page_Scan_Activity = 0x0C1C,
            HCI_Write_Page_Scan_Type = 0x0C47,
            HCI_Write_Inquiry_Scan_Activity = 0x0C1E,
            HCI_Write_Inquiry_Scan_Type = 0x0C43,
            HCI_Write_Class_of_Device = 0x0C24,
            HCI_Write_Extended_Inquiry_Response = 0x0C52,
            HCI_Write_Local_Name = 0x0C13,
            HCI_Set_Event_Mask = 0x0C01,
            HCI_IO_Capability_Request_Reply = 0x042B,
            HCI_User_Confirmation_Request_Reply = 0x042C,
            HCI_Set_Connection_Encryption = 0x0413,
            HCI_Authentication_Requested = 0x0411,
            HCI_Change_Connection_Link_Key = 0x0415,
            HCI_Read_Stored_Link_Key = 0x0C0D,
            HCI_Write_Stored_Link_Key = 0x0C11,
            HCI_Delete_Stored_Link_Key = 0x0C12
        }

        public enum Event : byte
        {
            HCI_Inquiry_Complete_EV = 0x01,
            HCI_Inquiry_Result_EV = 0x02,
            HCI_Connection_Complete_EV = 0x03,
            HCI_Connection_Request_EV = 0x04,
            HCI_Disconnection_Complete_EV = 0x05,
            HCI_Authentication_Complete_EV = 0x06,
            HCI_Remote_Name_Request_Complete_EV = 0x07,
            HCI_Encryption_Change_EV = 0x08,
            HCI_Change_Connection_Link_Key_Complete_EV = 0x09,
            HCI_Master_Link_Key_Complete_EV = 0x0A,
            HCI_Read_Remote_Supported_Features_Complete_EV = 0x0B,
            HCI_Read_Remote_Version_Information_Complete_EV = 0x0C,
            HCI_QoS_Setup_Complete_EV = 0x0D,
            HCI_Command_Complete_EV = 0x0E,
            HCI_Command_Status_EV = 0x0F,
            HCI_Hardware_Error_EV = 0x10,
            HCI_Flush_Occurred_EV = 0x11,
            HCI_Role_Change_EV = 0x12,
            HCI_Number_Of_Completed_Packets_EV = 0x13,
            HCI_Mode_Change_EV = 0x14,
            HCI_Return_Link_Keys_EV = 0x15,
            HCI_PIN_Code_Request_EV = 0x16,
            HCI_Link_Key_Request_EV = 0x17,
            HCI_Link_Key_Notification_EV = 0x18,
            HCI_Loopback_Command_EV = 0x19,
            HCI_Data_Buffer_Overflow_EV = 0x1A,
            HCI_Max_Slots_Change_EV = 0x1B,
            HCI_Read_Clock_Offset_Complete_EV = 0x1C,
            HCI_Connection_Packet_Type_Changed_EV = 0x1D,
            HCI_QoS_Violation_EV = 0x1E,
            HCI_Page_Scan_Repetition_Mode_Change_EV = 0x20,
            HCI_Flow_Specification_Complete_EV = 0x21,
            HCI_Inquiry_Result_With_RSSI_EV = 0x22,
            HCI_Read_Remote_Extended_Features_Complete_EV = 0x23,
            HCI_Synchronous_Connection_Complete_EV = 0x2C,
            HCI_Synchronous_Connection_Changed_EV = 0x2D,
            HCI_Sniff_Subrating_EV = 0x2E,
            HCI_Extended_Inquiry_Result_EV = 0x2F,
            HCI_IO_Capability_Request_EV = 0x31,
            HCI_IO_Capability_Response_EV = 0x32,
            HCI_User_Confirmation_Request_EV = 0x33,
            HCI_Simple_Pairing_Complete_EV = 0x36
        }
    }
}
