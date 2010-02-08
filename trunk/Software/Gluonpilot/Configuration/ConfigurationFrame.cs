﻿/*!
 *   ConfigurationFrame.c
 *   Manages the User Control for changing gluonpilot configuration.
 *   
 *   @author  Tom Pycke
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Communication;
using Communication.Frames.Configuration;
using Communication.Frames.Incoming;
using Configuration;

namespace Gluonpilot
{
    public partial class ConfigurationFrame : UserControl
    {
        private SerialCommunication _serial;

        private ConfigurationModel _model;

        public ConfigurationFrame(ConfigurationModel model)
        {
            InitializeComponent();
            _model = model;
        }

        public ConfigurationFrame()
        {
            InitializeComponent();
            _model = new ConfigurationModel();
        }

        public ConfigurationModel Model
        {
            get
            {
                return _model;
            }
            set
            {
                _model = value;
                ReadModel();
            }
        }

        /*!
         *    Reads _model and maps it's value to the Form's components
         */
        private void ReadModel()
        {
            _tbAccXNeutral.Text = _model.NeutralAccX.ToString();
            _tbAccYNeutral.Text = _model.NeutralAccY.ToString();
            _tbAccZNeutral.Text = _model.NeutralAccZ.ToString();
            _tbGyroXNeutral.Text = _model.NeutralGyroX.ToString();
            _tbGyroYNeutral.Text = _model.NeutralGyroY.ToString();
            _tbGyroZNeutral.Text = _model.NeutralGyroZ.ToString();

            _nud_gpsbasic_telemetry.Value = _model.TelemetryGpsBasic;
            _nud_gyroaccraw_telemetry.Value = _model.TelemetryGyroAccRaw;
            _nud_gyroaccproc_telemetry.Value = _model.TelemetryGyroAccProc;
            _nud_ppm_telemetry.Value = _model.TelemetryPpm;
            _nud_pressuretemp_telemetry.Value = _model.TelemetryPressureTemp;

            _tb_initial_baudrate.Text = _model.GpsInitialBaudrate.ToString();
            _tb_operational_baudrate.Text = _model.GpsOperationalBaudrate.ToString();

            _pid_pitch_to_elevator.SetModel(_model.Pitch2ElevatorPidModel);
            _pid_roll_to_aileron.SetModel(_model.Roll2AileronPidModel);
            _pid_heading_to_roll.SetModel(_model.Heading2RollPidModel);

            _cb_reverse_servo1.Checked = _model.ReverseServo1;
            _cb_reverse_servo2.Checked = _model.ReverseServo2;
            _cb_reverse_servo3.Checked = _model.ReverseServo3;
            _cb_reverse_servo4.Checked = _model.ReverseServo4;
            _cb_reverse_servo5.Checked = _model.ReverseServo5;
            _cb_reverse_servo6.Checked = _model.ReverseServo6;

            switch (_model.ChannelRoll)
            {
                case 1: _rbRollCh1.Checked = true; break;
                case 2: _rbRollCh2.Checked = true; break;
                case 3: _rbRollCh3.Checked = true; break;
                case 4: _rbRollCh4.Checked = true; break;
                case 5: _rbRollCh5.Checked = true; break;
                case 6: _rbRollCh6.Checked = true; break;
                case 7: _rbRollCh7.Checked = true; break;
            }
            switch (_model.ChannelPitch)
            {
                case 1: _rbPitchCh1.Checked = true; break;
                case 2: _rbPitchCh2.Checked = true; break;
                case 3: _rbPitchCh3.Checked = true; break;
                case 4: _rbPitchCh4.Checked = true; break;
                case 5: _rbPitchCh5.Checked = true; break;
                case 6: _rbPitchCh6.Checked = true; break;
                case 7: _rbPitchCh7.Checked = true; break;
            }
            switch (_model.ChannelYaw)
            {
                case 1: _rbYawCh1.Checked = true; break;
                case 2: _rbYawCh2.Checked = true; break;
                case 3: _rbYawCh3.Checked = true; break;
                case 4: _rbYawCh4.Checked = true; break;
                case 5: _rbYawCh5.Checked = true; break;
                case 6: _rbYawCh6.Checked = true; break;
                case 7: _rbYawCh7.Checked = true; break;
            }
            switch (_model.ChannelMotor)
            {
                case 1: _rbMotorCh1.Checked = true; break;
                case 2: _rbMotorCh2.Checked = true; break;
                case 3: _rbMotorCh3.Checked = true; break;
                case 4: _rbMotorCh4.Checked = true; break;
                case 5: _rbMotorCh5.Checked = true; break;
                case 6: _rbMotorCh6.Checked = true; break;
                case 7: _rbMotorCh7.Checked = true; break;
            }
            switch (_model.ChannelAp)
            {
                case 1: _rbApCh1.Checked = true; break;
                case 2: _rbApCh2.Checked = true; break;
                case 3: _rbApCh3.Checked = true; break;
                case 4: _rbApCh4.Checked = true; break;
                case 5: _rbApCh5.Checked = true; break;
                case 6: _rbApCh6.Checked = true; break;
                case 7: _rbApCh7.Checked = true; break;
            }
        }

        /*!
         *    Converts to _model to AllConfig communication frame.
         */
        public AllConfig ToAllConfig()
        {
            AllConfig ac = new AllConfig();
            ac.acc_x_neutral = _model.NeutralAccX;
            ac.acc_y_neutral = _model.NeutralAccY;
            ac.acc_z_neutral = _model.NeutralAccZ;
            ac.gyro_x_neutral = _model.NeutralGyroX;
            ac.gyro_y_neutral = _model.NeutralGyroY;
            ac.gyro_z_neutral = _model.NeutralGyroZ;

            ac.channel_pitch = _model.ChannelPitch;
            ac.channel_roll = _model.ChannelRoll;
            ac.channel_motor = _model.ChannelMotor;
            ac.channel_yaw = _model.ChannelYaw;
            ac.channel_ap = _model.ChannelAp;

            ac.telemetry_basicgps = _model.TelemetryGpsBasic;
            ac.telemetry_gyroaccraw = _model.TelemetryGyroAccRaw;
            ac.telemetry_gyroaccproc = _model.TelemetryGyroAccProc;
            ac.telemetry_ppm = _model.TelemetryPpm;
            ac.telemetry_pressuretemp = _model.TelemetryPressureTemp;

            ac.gps_initial_baudrate = _model.GpsInitialBaudrate;

            ac.pid_pitch2elevator_p = _model.Pitch2ElevatorPidModel.P;
            ac.pid_pitch2elevator_i = _model.Pitch2ElevatorPidModel.I;
            ac.pid_pitch2elevator_d = _model.Pitch2ElevatorPidModel.D;
            ac.pid_pitch2elevator_imin = _model.Pitch2ElevatorPidModel.IMin;
            ac.pid_pitch2elevator_imax = _model.Pitch2ElevatorPidModel.IMax;
            ac.pid_pitch2elevator_dmin = _model.Pitch2ElevatorPidModel.DMin;

            ac.pid_roll2aileron_p = _model.Roll2AileronPidModel.P;
            ac.pid_roll2aileron_i = _model.Roll2AileronPidModel.I;
            ac.pid_roll2aileron_d = _model.Roll2AileronPidModel.D;
            ac.pid_roll2aileron_imin = _model.Roll2AileronPidModel.IMin;
            ac.pid_roll2aileron_imax = _model.Roll2AileronPidModel.IMax;
            ac.pid_roll2aileron_dmin = _model.Roll2AileronPidModel.DMin;

            ac.pid_heading2roll_p = _model.Heading2RollPidModel.P;
            ac.pid_heading2roll_i = _model.Heading2RollPidModel.I;
            ac.pid_heading2roll_d = _model.Heading2RollPidModel.D;
            ac.pid_heading2roll_imin = _model.Heading2RollPidModel.IMin;
            ac.pid_heading2roll_imax = _model.Heading2RollPidModel.IMax;
            ac.pid_heading2roll_dmin = _model.Heading2RollPidModel.DMin;

            ac.servo_reverse[0] = _model.ReverseServo1;
            ac.servo_reverse[1] = _model.ReverseServo2;
            ac.servo_reverse[2] = _model.ReverseServo3;
            ac.servo_reverse[3] = _model.ReverseServo4;
            ac.servo_reverse[4] = _model.ReverseServo5;
            ac.servo_reverse[5] = _model.ReverseServo6;

            return ac;
        }

        /*!
         *    Use serial as SerialCommunication and register our methods at the events
         */
        public void Connect(SerialCommunication serial)
        {
            _serial = serial;
            _serial.GyroAccRawCommunicationReceived += new SerialCommunication_CSV.ReceiveGyroAccRawCommunicationFrame(ReceiveGyroAccRaw);
            _serial.GyroAccProcCommunicationReceived += new SerialCommunication_CSV.ReceiveGyroAccProcCommunicationFrame(ReceiveGyroAccProc);
            _serial.PressureTempCommunicationReceived += new SerialCommunication_CSV.ReceivePressureTempCommunicationFrame(ReceivePressureTemp);
            _serial.AllConfigCommunicationReceived += new SerialCommunication_CSV.ReceiveAllConfigCommunicationFrame(ReceiveAllConfig);
            _serial.RcInputCommunicationReceived += new SerialCommunication_CSV.ReceiveRcInputCommunicationFrame(ReceiveRcInput);
            _serial.GpsBasicCommunicationReceived += new SerialCommunication.ReceiveGpsBasicCommunicationFrame(ReceiveGpsBasic);
        }

        private void ReceiveAllConfig(AllConfig ac)
        {
            this.BeginInvoke(new D_ReceiveAllConfig(AllConfig), new object[] { ac });
        }
        private delegate void D_ReceiveAllConfig(AllConfig ac);
        private void AllConfig(AllConfig ac)
        {
            _model.NeutralAccX = ac.acc_x_neutral;
            _model.NeutralAccY = ac.acc_y_neutral;
            _model.NeutralAccZ = ac.acc_z_neutral;
            _model.NeutralGyroX = ac.gyro_x_neutral;
            _model.NeutralGyroY = ac.gyro_y_neutral;
            _model.NeutralGyroZ = ac.gyro_z_neutral;

            _model.TelemetryGpsBasic = ac.telemetry_basicgps;
            _model.TelemetryGyroAccRaw = ac.telemetry_gyroaccraw;
            _model.TelemetryGyroAccProc = ac.telemetry_gyroaccproc;
            _model.TelemetryPpm = ac.telemetry_ppm;
            _model.TelemetryPressureTemp = ac.telemetry_pressuretemp;

            _model.ChannelAp = ac.channel_ap;
            _model.ChannelPitch = ac.channel_pitch;
            _model.ChannelRoll = ac.channel_roll;
            _model.ChannelYaw = ac.channel_yaw;
            _model.ChannelMotor = ac.channel_motor;

            _model.GpsInitialBaudrate = ac.gps_initial_baudrate;
            _model.GpsOperationalBaudrate = ac.gps_operational_baudrate;

            _model.Pitch2ElevatorPidModel = new PidModel(ac.pid_pitch2elevator_p,
                                                         ac.pid_pitch2elevator_i,
                                                         ac.pid_pitch2elevator_d,
                                                         ac.pid_pitch2elevator_imin,
                                                         ac.pid_pitch2elevator_imax,
                                                         ac.pid_pitch2elevator_dmin);
            _model.Roll2AileronPidModel = new PidModel(ac.pid_roll2aileron_p,
                                                         ac.pid_roll2aileron_i,
                                                         ac.pid_roll2aileron_d,
                                                         ac.pid_roll2aileron_imin,
                                                         ac.pid_roll2aileron_imax,
                                                         ac.pid_roll2aileron_dmin);
            _model.Heading2RollPidModel = new PidModel(ac.pid_heading2roll_p,
                                                         ac.pid_heading2roll_i,
                                                         ac.pid_heading2roll_d,
                                                         ac.pid_heading2roll_imin,
                                                         ac.pid_heading2roll_imax,
                                                         ac.pid_heading2roll_dmin);
            _model.ReverseServo1 = ac.servo_reverse[0];
            _model.ReverseServo2 = ac.servo_reverse[1];
            _model.ReverseServo3 = ac.servo_reverse[2];
            _model.ReverseServo4 = ac.servo_reverse[3];
            _model.ReverseServo5 = ac.servo_reverse[4];
            _model.ReverseServo6 = ac.servo_reverse[5];
            
            ReadModel();
        }


        private void ReceiveRcInput(RcInput rc)
        {
            this.BeginInvoke(new D_ReceiveRcInput(RcTransmitterInput), new object[] { rc });
        }
        private delegate void D_ReceiveRcInput(RcInput rc);
        private void RcTransmitterInput(RcInput rc)
        {
            _pbCh1.Value = InBetween(rc.GetPwm(1), 500, 2200);
            _lblCh1Ms.Text = rc.GetPwm(1).ToString();
            _pbCh2.Value = InBetween(rc.GetPwm(2), 500, 2200);
            _lblCh2Ms.Text = rc.GetPwm(2).ToString();
            _pbCh3.Value = InBetween(rc.GetPwm(3), 500, 2200);
            _lblCh3Ms.Text = rc.GetPwm(3).ToString();
            _pbCh4.Value = InBetween(rc.GetPwm(4), 500, 2200);
            _lblCh4Ms.Text = rc.GetPwm(4).ToString();
            _pbCh5.Value = InBetween(rc.GetPwm(5), 500, 2200);
            _lblCh5Ms.Text = rc.GetPwm(5).ToString();
            _pbCh6.Value = InBetween(rc.GetPwm(6), 500, 2200);
            _lblCh6Ms.Text = rc.GetPwm(6).ToString();
            _pbCh7.Value = InBetween(rc.GetPwm(7), 500, 2200);
            _lblCh7Ms.Text = rc.GetPwm(7).ToString();
        }
        private int InBetween(int x, int a, int b)
        {
            if (x < a)
                return a;
            if (x > b)
                return b;
            return x;
        }

        private void ReceiveGyroAccRaw(GyroAccRaw ga)
        {
            this.BeginInvoke(new D_UpdateGyroAccRaw(UpdateGyroAccRaw), new object[] { ga });
        }
        private delegate void D_UpdateGyroAccRaw(GyroAccRaw ga);
        private void UpdateGyroAccRaw(GyroAccRaw ga)
        {
            _tbAccXRaw.Text = ga.AccXRaw.ToString();
            _tbAccYRaw.Text = ga.AccYRaw.ToString();
            _tbAccZRaw.Text = ga.AccZRaw.ToString();

            _tbGyroXRaw.Text = ga.GyroXRaw.ToString();
            _tbGyroYRaw.Text = ga.GyroYRaw.ToString();
            _tbGyroZRaw.Text = ga.GyroZRaw.ToString();
        }

        private void ReceiveGyroAccProc(GyroAccProcessed ga)
        {
            this.BeginInvoke(new D_UpdateGyroAccProc(UpdateGyroAccProc), new object[] { ga });
        }
        private delegate void D_UpdateGyroAccProc(GyroAccProcessed ga);
        private void UpdateGyroAccProc(GyroAccProcessed ga)
        {
            _tbAccX.Text = ga.AccX.ToString();
            _tbAccY.Text = ga.AccY.ToString();
            _tbAccZ.Text = ga.AccZ.ToString();

            _tbSumAcc.Text = Math.Sqrt(ga.AccX * ga.AccX + ga.AccY * ga.AccY + ga.AccZ * ga.AccZ).ToString();

            _tbGyroX.Text = ga.GyroX.ToString();
            _tbGyroY.Text = ga.GyroY.ToString();
            _tbGyroZ.Text = ga.GyroZ.ToString();
        }


        private void ReceivePressureTemp(PressureTemp pt)
        {
            string s = "";
            this.BeginInvoke(new D_UpdateScp1000(UpdateScp1000), new object[] { pt });
        }
        private delegate void D_UpdateScp1000(PressureTemp pt);
        private void UpdateScp1000(PressureTemp pt)
        {
            _tbPressure.Text = pt.Pressure.ToString();
            _tbTemperature.Text = pt.Temperature.ToString();
            _tbHeight.Text = pt.Height.ToString();
        }

        private void _btn_sensors_current_to_neutral_Click(object sender, EventArgs e)
        {
            _tbAccXNeutral.Text = _tbAccXRaw.Text;
            _tbAccYNeutral.Text = _tbAccYRaw.Text;
            _tbAccZNeutral.Text = _tbAccZRaw.Text;
        }

        private void _btn_use_current_gyro_Click(object sender, EventArgs e)
        {
            _tbGyroXNeutral.Text = _tbGyroXRaw.Text;
            _tbGyroYNeutral.Text = _tbGyroYRaw.Text;
            _tbGyroZNeutral.Text = _tbGyroZRaw.Text;
        }

        private void _tbGyroNeutral_TextChanged(object sender, EventArgs e)
        {
            if (sender == _tbGyroXNeutral)
                _model.NeutralGyroX = _tbGyroXNeutral.IntValue;
            if (sender == _tbGyroYNeutral)
                _model.NeutralGyroY = _tbGyroYNeutral.IntValue;
            if (sender == _tbGyroZNeutral)
                _model.NeutralGyroZ = _tbGyroZNeutral.IntValue;
        }

        private void _tbAccNeutral_TextChanged(object sender, EventArgs e)
        {
            if (sender == _tbAccXNeutral)
                _model.NeutralAccX = _tbAccXNeutral.IntValue;
            if (sender == _tbAccYNeutral)
                _model.NeutralAccY = _tbAccYNeutral.IntValue;
            if (sender == _tbAccZNeutral)
                _model.NeutralAccZ = _tbAccZNeutral.IntValue;
        }


#region RcInput tab page

        private void RollChannel_CheckedChanged(object sender, EventArgs e)
        {
            if (_rbRollCh1.Checked)
                _model.ChannelRoll = 1;
            else if (_rbRollCh2.Checked)
                _model.ChannelRoll = 2;
            else if (_rbRollCh3.Checked)
                _model.ChannelRoll = 3;
            else if (_rbRollCh4.Checked)
                _model.ChannelRoll = 4;
            else if (_rbRollCh5.Checked)
                _model.ChannelRoll = 5;
            else if (_rbRollCh6.Checked)
                _model.ChannelRoll = 6;
            else if (_rbRollCh7.Checked)
                _model.ChannelRoll = 7;
        }

        private void PitchChannel_CheckedChanged(object sender, EventArgs e)
        {
            if (_rbPitchCh1.Checked)
                _model.ChannelPitch = 1;
            else if (_rbPitchCh2.Checked)
                _model.ChannelPitch = 2;
            else if (_rbPitchCh3.Checked)
                _model.ChannelPitch = 3;
            else if (_rbPitchCh4.Checked)
                _model.ChannelPitch = 4;
            else if (_rbPitchCh5.Checked)
                _model.ChannelPitch = 5;
            else if (_rbPitchCh6.Checked)
                _model.ChannelPitch = 6;
            else if (_rbPitchCh7.Checked)
                _model.ChannelPitch = 7;
        }

        private void YawChannel_CheckedChanged(object sender, EventArgs e)
        {
            if (_rbYawCh1.Checked)
                _model.ChannelYaw = 1;
            else if (_rbYawCh2.Checked)
                _model.ChannelYaw = 2;
            else if (_rbYawCh3.Checked)
                _model.ChannelYaw = 3;
            else if (_rbYawCh4.Checked)
                _model.ChannelYaw = 4;
            else if (_rbYawCh5.Checked)
                _model.ChannelYaw = 5;
            else if (_rbYawCh6.Checked)
                _model.ChannelYaw = 6;
            else if (_rbYawCh7.Checked)
                _model.ChannelYaw = 7;
        }

        private void MotorChannel_CheckedChanged(object sender, EventArgs e)
        {
            if (_rbMotorCh1.Checked)
                _model.ChannelMotor = 1;
            else if (_rbMotorCh2.Checked)
                _model.ChannelMotor = 2;
            else if (_rbMotorCh3.Checked)
                _model.ChannelMotor = 3;
            else if (_rbMotorCh4.Checked)
                _model.ChannelMotor = 4;
            else if (_rbMotorCh5.Checked)
                _model.ChannelMotor = 5;
            else if (_rbMotorCh6.Checked)
                _model.ChannelMotor = 6;
            else if (_rbMotorCh7.Checked)
                _model.ChannelMotor = 7;
        }

        private void ApChannel_CheckedChanged(object sender, EventArgs e)
        {
            if (_rbApCh1.Checked)
                _model.ChannelAp = 1;
            else if (_rbApCh2.Checked)
                _model.ChannelAp = 2;
            else if (_rbApCh3.Checked)
                _model.ChannelAp = 3;
            else if (_rbApCh4.Checked)
                _model.ChannelAp = 4;
            else if (_rbApCh5.Checked)
                _model.ChannelAp = 5;
            else if (_rbApCh6.Checked)
                _model.ChannelAp = 6;
            else if (_rbApCh7.Checked)
                _model.ChannelAp = 7;
        }
#endregion

#region Telemetry tab page
       
        private void _nud_gyroaccproc_telemetry_ValueChanged(object sender, EventArgs e)
        {
            _model.TelemetryGyroAccProc = (int)_nud_gyroaccproc_telemetry.Value;
        }

        private void _nud_ppm_telemetry_ValueChanged(object sender, EventArgs e)
        {
            _model.TelemetryPpm = (int)_nud_ppm_telemetry.Value;
        }

        private void _nud_pressuretemp_telemetry_ValueChanged(object sender, EventArgs e)
        {
            _model.TelemetryPressureTemp = (int)_nud_pressuretemp_telemetry.Value;
        }

        private void _nud_gpsbasic_telemetry_ValueChanged(object sender, EventArgs e)
        {
            _model.TelemetryGpsBasic = (int)_nud_gpsbasic_telemetry.Value;
        }
        
        private void _nud_gyroacc_telemetry_ValueChanged(object sender, EventArgs e)
        {
            _model.TelemetryGyroAccRaw = (int)_nud_gyroaccraw_telemetry.Value;
        }
#endregion

#region Servos tab page

        private void _cb_reverse_servo_Click(object sender, EventArgs e)
        {
            if (sender == _cb_reverse_servo1)
                _model.ReverseServo1 = _cb_reverse_servo1.Checked;
            if (sender == _cb_reverse_servo2)
                _model.ReverseServo2 = _cb_reverse_servo2.Checked;
            if (sender == _cb_reverse_servo3)
                _model.ReverseServo3 = _cb_reverse_servo3.Checked;
            if (sender == _cb_reverse_servo4)
                _model.ReverseServo4 = _cb_reverse_servo4.Checked;
            if (sender == _cb_reverse_servo5)
                _model.ReverseServo5 = _cb_reverse_servo5.Checked;
            if (sender == _cb_reverse_servo6)
                _model.ReverseServo6 = _cb_reverse_servo6.Checked;
        }

#endregion

#region GPS tab page

        private void ReceiveGpsBasic(GpsBasic gb)
        {
            string s = "";
            this.BeginInvoke(new D_UpdateGpsBasic(UpdateGpsBasic), new object[] { gb });
        }
        private delegate void D_UpdateGpsBasic(GpsBasic gb);
        private void UpdateGpsBasic(GpsBasic gb)
        {
            if (gb.Status == 1)
                _rb_gps_status_active.Checked = true;
            else if(gb.Status == 0)
                _rb_gps_status_void.Checked = true;

            _tb_gps_numsat.Text = gb.NumberOfSatellites.ToString();
            _tb_gps_latitude.Text = gb.Latitude.ToString();
            _tb_gps_longitude.Text = gb.Longitude.ToString();
        }

        private void _tb_operational_baudrate_TextChanged(object sender, EventArgs e)
        {
            _model.GpsOperationalBaudrate = _tb_operational_baudrate.IntValue;
        }

        private void _tb_initial_baudrate_TextChanged(object sender, EventArgs e)
        {
            _model.GpsInitialBaudrate = _tb_initial_baudrate.IntValue;
        }

        private void _llGoogleMaps_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://maps.google.com/maps?ll=" + 
                (double.Parse(_tb_gps_latitude.Text) / 3.14159 * 180.0).ToString(System.Globalization.CultureInfo.InvariantCulture) + 
                "," +
                (double.Parse(_tb_gps_longitude.Text) / 3.14159 * 180.0).ToString(System.Globalization.CultureInfo.InvariantCulture) + "&spn=0.005025,0.008500&t=h");
        }

#endregion


        private void _cbControlMix_SelectedIndexChanged(object sender, EventArgs e)
        {
            _lblControlMixInfo.Text = "1: Up/Down, 2: Aileron1, 3: Aileron2, 4: Motor";
        }

        private void _pid_pitch_to_elevator_IsChanged(object sender, EventArgs e)
        {
            _model.Pitch2ElevatorPidModel = _pid_pitch_to_elevator.Model;
        }

        private void _pid_roll_to_aileron_IsChanged(object sender, EventArgs e)
        {
            _model.Roll2AileronPidModel = _pid_roll_to_aileron.Model;
        }

        private void _pid_heading_to_roll_IsChanged(object sender, EventArgs e)
        {
            _model.Heading2RollPidModel = _pid_heading_to_roll.Model;
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.gluonpilot.com/wiki/Config_telemetry");
        }

        private void _llConfigSensors_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.gluonpilot.com/wiki/Config_sensors");
        }

    }
}
