Public Class Form1
    Dim myPort As Array
    Delegate Sub SetTextCallback(ByVal [text] As String)

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        'Integration time = (ATIME + 1) x (ASTEP + 1) x 2.78µs
        Dim val As Double
        val = (NumericUpDown1.Value + 1) * (NumericUpDown2.Value + 1) * 2.78
        Label19.Text = val.ToString()
        If SerialPort1.IsOpen = True Then
            NumericUpDown1.Enabled = True
            SerialPort1.WriteLine("_F:Time=" & NumericUpDown1.Value)
        Else
            NumericUpDown1.Enabled = False
        End If
    End Sub

    Private Sub NumericUpDown2_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown2.ValueChanged
        Dim val As Double
        val = (NumericUpDown1.Value + 1) * (NumericUpDown2.Value + 1) * 2.78
        Label19.Text = val.ToString()
        If SerialPort1.IsOpen = True Then
            NumericUpDown2.Enabled = True
            SerialPort1.WriteLine("_F:Step=" & NumericUpDown2.Value)
        Else
            NumericUpDown2.Enabled = False
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' Show all available COM ports.
        If IsNothing(ComboBox1.SelectedItem) Then
            myPort = IO.Ports.SerialPort.GetPortNames()
            ComboBox1.Items.AddRange(myPort)
            Button2.Enabled = True
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        SerialPort1.PortName = ComboBox1.Text
        SerialPort1.BaudRate = ComboBox2.Text
        SerialPort1.DataBits = ComboBox3.Text
        SerialPort1.StopBits = ComboBox5.Text

        Select Case ComboBox4.Text
            Case "none"
                SerialPort1.Parity = IO.Ports.Parity.None
            Case "odd"
                SerialPort1.Parity = IO.Ports.Parity.Odd
            Case "even"
                SerialPort1.Parity = IO.Ports.Parity.Even
            Case "mark"
                SerialPort1.Parity = IO.Ports.Parity.Mark
            Case "space"
                SerialPort1.Parity = IO.Ports.Parity.Space
        End Select

        Select Case ComboBox6.Text
            Case "none"
                SerialPort1.Handshake = IO.Ports.Handshake.None
            Case "RTS/CTS"
                SerialPort1.Handshake = IO.Ports.Handshake.RequestToSend
            Case "XON/XOFF"
                SerialPort1.Handshake = IO.Ports.Handshake.XOnXOff
            Case "RTS/CTS+XON/XOFF"
                SerialPort1.Handshake = IO.Ports.Handshake.RequestToSendXOnXOff
        End Select

        SerialPort1.Open()

        If SerialPort1.IsOpen = True Then
            ComboBox7.Enabled = True
            ComboBox8.Enabled = True
            NumericUpDown1.Enabled = True
            NumericUpDown2.Enabled = True

            Button2.Enabled = False
            Button1.Enabled = False
            Button4.Enabled = True
            Button2.Text = "Connected"
        Else
            Button2.Text = "ERROR !"
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If SerialPort1.IsOpen = True Then
            SendToAll()
        Else
            Button3.Enabled = False
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        SerialPort1.Close()
        If SerialPort1.IsOpen = False Then
            Button1.Enabled = True
            Button2.Enabled = True
            Button2.Text = "Connect"
        End If
    End Sub

    Private Sub ComboBox8_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox8.SelectedIndexChanged
        If SerialPort1.IsOpen = True Then
            ComboBox8.Enabled = True
            Select Case ComboBox8.Text
                Case "0.5"
                    SerialPort1.WriteLine("_F:Gain=0" & NumericUpDown2.Value)
                Case "1"
                    SerialPort1.WriteLine("_F:Gain=1" & NumericUpDown2.Value)
                Case "2"
                    SerialPort1.WriteLine("_F:Gain=2" & NumericUpDown2.Value)
                Case "4"
                    SerialPort1.WriteLine("_F:Gain=3" & NumericUpDown2.Value)
                Case "8"
                    SerialPort1.WriteLine("_F:Gain=4" & NumericUpDown2.Value)
                Case "16"
                    SerialPort1.WriteLine("_F:Gain=5" & NumericUpDown2.Value)
                Case "32"
                    SerialPort1.WriteLine("_F:Gain=6" & NumericUpDown2.Value)
                Case "64"
                    SerialPort1.WriteLine("_F:Gain=7" & NumericUpDown2.Value)
                Case "128"
                    SerialPort1.WriteLine("_F:Gain=8" & NumericUpDown2.Value)
                Case "256"
                    SerialPort1.WriteLine("_F:Gain=9" & NumericUpDown2.Value)
                Case "512"
                    SerialPort1.WriteLine("_F:Gain=10" & NumericUpDown2.Value)
            End Select
        Else
            ComboBox8.Enabled = False
        End If
    End Sub

    Private Sub ComboBox7_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox7.SelectedIndexChanged
        If SerialPort1.IsOpen() Then
            If ComboBox7.Text = 0 Then
                SerialPort1.WriteLine("_CurrentLed=30")
            Else
                SerialPort1.WriteLine("_CurrentLed=" & ComboBox7.Text)
            End If
        Else
            ComboBox7.Enabled = False
        End If
    End Sub
    Private Sub SerialPort1_DataReceived(sender As System.Object, e As System.IO.Ports.SerialDataReceivedEventArgs) Handles SerialPort1.DataReceived
        ReceivedText(SerialPort1.ReadExisting())
    End Sub

    Private Sub ReceivedText(ByVal [text] As String)
        If Me.RichTextBox2.InvokeRequired Then
            Dim x As New SetTextCallback(AddressOf ReceivedText)
            Me.Invoke(x, New Object() {[text]})
        Else
            Me.RichTextBox2.Text &= [text]
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        SerialPort1.Write(RichTextBox1.Text & vbCr)
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        RichTextBox2.Text = ("")
    End Sub

    Private Sub RichTextBox2_TextChanged(sender As Object, e As EventArgs) Handles RichTextBox2.TextChanged
        Dim equalsPos, i, svals(10), svmax As Integer
        i = 0
        Dim arrOfData() As String
        arrOfData = Split("F1,F2,F3,F4,F5,F6,F7,F8,Clear,NIR,EOF", ",")

        If RichTextBox2.TextLength > 40 Then
            TextBox2.Text = ""
            For Each line As String In RichTextBox2.Lines
                equalsPos = line.IndexOf(":") + 1
                arrOfData(i) = (line.Substring(equalsPos, line.Length - equalsPos))
                i += 1
            Next

            TextBox2.Text = arrOfData(0) & ", " & arrOfData(1) & ", " & arrOfData(2) & ", " & arrOfData(3) &
                    ", " & arrOfData(4) & ", " & arrOfData(5) & ", " & arrOfData(6) & ", " & arrOfData(7) &
                    ", " & arrOfData(8) & ", " & arrOfData(9) & ", " & arrOfData(10)

            For i = 0 To 9
                svals(i) = CInt(arrOfData(i))
            Next

            If CheckBox3.Checked Then
                Dim timeVal As String = Format(DateTime.Now, "hh:mm:ss")
                Me.Chart1.Series("Clear").Points.AddXY(timeVal, svals(8))
                Me.Chart1.Series("Violet").Points.AddXY(timeVal, svals(0))
                Me.Chart1.Series("Blue").Points.AddXY(timeVal, svals(1))
                Me.Chart1.Series("Cyan").Points.AddXY(timeVal, svals(2))
                Me.Chart1.Series("Aqua").Points.AddXY(timeVal, svals(3))
                Me.Chart1.Series("Green").Points.AddXY(timeVal, svals(4))
                Me.Chart1.Series("Yellow").Points.AddXY(timeVal, svals(5))
                Me.Chart1.Series("Orange").Points.AddXY(timeVal, svals(6))
                Me.Chart1.Series("Red").Points.AddXY(timeVal, svals(7))
                Me.Chart1.Series("NIR").Points.AddXY(timeVal, svals(9))
            End If

            svmax = svals.Max() + (0.1 * svals.Max())


            ProgressBar11.Maximum = svmax
            ProgressBar11.Value = CInt(arrOfData(8))

            ProgressBar1.Maximum = svmax
            ProgressBar1.Value = CInt(arrOfData(0))

            ProgressBar2.Maximum = svmax
            ProgressBar2.Value = CInt(arrOfData(1))

            ProgressBar3.Maximum = svmax
            ProgressBar3.Value = CInt(arrOfData(2))

            ProgressBar4.Maximum = svmax
            ProgressBar4.Value = CInt(arrOfData(3))

            ProgressBar5.Maximum = svmax
            ProgressBar5.Value = CInt(arrOfData(4))

            ProgressBar6.Maximum = svmax
            ProgressBar6.Value = CInt(arrOfData(5))

            ProgressBar7.Maximum = svmax
            ProgressBar7.Value = CInt(arrOfData(6))

            ProgressBar8.Maximum = svmax
            ProgressBar8.Value = CInt(arrOfData(7))

            ProgressBar9.Maximum = svmax
            ProgressBar9.Value = CInt(arrOfData(9))
        End If
    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        If CheckBox4.Checked = True Then
            Timer1.Interval = TextBox1.Text * 1000
            Timer1.Start()
        Else
            Timer1.Stop()
        End If
    End Sub

    Private Sub SendToAll()
        If SerialPort1.IsOpen = True Then
            RichTextBox2.Text = ""
            SerialPort1.WriteLine("_F:All")
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        SendToAll()
    End Sub
End Class