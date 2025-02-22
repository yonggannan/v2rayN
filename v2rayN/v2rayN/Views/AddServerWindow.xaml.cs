﻿using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using v2rayN.Base;
using v2rayN.Handler;
using v2rayN.Mode;
using v2rayN.Resx;
using v2rayN.ViewModels;

namespace v2rayN.Views
{
    public partial class AddServerWindow
    {

        public AddServerWindow(ProfileItem profileItem)
        {
            InitializeComponent();
            this.Loaded += Window_Loaded;
            cmbNetwork.SelectionChanged += CmbNetwork_SelectionChanged;
            cmbStreamSecurity.SelectionChanged += CmbStreamSecurity_SelectionChanged;

            ViewModel = new AddServerViewModel(profileItem, this);

            Global.coreTypes.ForEach(it =>
            {
                cmbCoreType.Items.Add(it);
            });
            cmbCoreType.Items.Add(string.Empty);

            cmbStreamSecurity.Items.Add(string.Empty);
            cmbStreamSecurity.Items.Add(Global.StreamSecurity);

            Global.networks.ForEach(it =>
            {
                cmbNetwork.Items.Add(it);
            });
            Global.fingerprints.ForEach(it =>
            {
                cmbFingerprint.Items.Add(it);
            });
            Global.allowInsecures.ForEach(it =>
            {
                cmbAllowInsecure.Items.Add(it);
            });
            Global.alpns.ForEach(it =>
            {
                cmbAlpn.Items.Add(it);
            });

            switch (profileItem.configType)
            {
                case EConfigType.VMess:
                    gridVMess.Visibility = Visibility.Visible;
                    Global.vmessSecuritys.ForEach(it =>
                    {
                        cmbSecurity.Items.Add(it);
                    });
                    if (profileItem.security.IsNullOrEmpty())
                    {
                        profileItem.security = Global.DefaultSecurity;
                    }
                    break;
                case EConfigType.Shadowsocks:
                    gridSs.Visibility = Visibility.Visible;
                    LazyConfig.Instance.GetShadowsocksSecuritys(profileItem).ForEach(it =>
                    {
                        cmbSecurity3.Items.Add(it);
                    });
                    break;
                case EConfigType.Socks:
                    gridSocks.Visibility = Visibility.Visible;
                    break;
                case EConfigType.VLESS:
                    gridVLESS.Visibility = Visibility.Visible;
                    cmbStreamSecurity.Items.Add(Global.StreamSecurityX);
                    Global.xtlsFlows.ForEach(it =>
                    {
                        cmbFlow5.Items.Add(it);
                    });
                    if (profileItem.security.IsNullOrEmpty())
                    {
                        profileItem.security = Global.None;
                    }
                    break;
                case EConfigType.Trojan:
                    gridTrojan.Visibility = Visibility.Visible;
                    cmbStreamSecurity.Items.Add(Global.StreamSecurityX);
                    Global.xtlsFlows.ForEach(it =>
                    {
                        cmbFlow6.Items.Add(it);
                    });
                    break;
            }

            gridTlsMore.Visibility = Visibility.Hidden;

            this.WhenActivated(disposables =>
            {
                this.Bind(ViewModel, vm => vm.SelectedSource.coreType, v => v.cmbCoreType.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedSource.remarks, v => v.txtRemarks.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedSource.address, v => v.txtAddress.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedSource.port, v => v.txtPort.Text).DisposeWith(disposables);

                switch (profileItem.configType)
                {
                    case EConfigType.VMess:
                        this.Bind(ViewModel, vm => vm.SelectedSource.id, v => v.txtId.Text).DisposeWith(disposables);
                        this.Bind(ViewModel, vm => vm.SelectedSource.alterId, v => v.txtAlterId.Text).DisposeWith(disposables);
                        this.Bind(ViewModel, vm => vm.SelectedSource.security, v => v.cmbSecurity.Text).DisposeWith(disposables);
                        break;
                    case EConfigType.Shadowsocks:
                        this.Bind(ViewModel, vm => vm.SelectedSource.id, v => v.txtId3.Text).DisposeWith(disposables);
                        this.Bind(ViewModel, vm => vm.SelectedSource.security, v => v.cmbSecurity3.Text).DisposeWith(disposables);
                        break;
                    case EConfigType.Socks:
                        this.Bind(ViewModel, vm => vm.SelectedSource.id, v => v.txtId4.Text).DisposeWith(disposables);
                        this.Bind(ViewModel, vm => vm.SelectedSource.security, v => v.txtSecurity4.Text).DisposeWith(disposables);
                        break;
                    case EConfigType.VLESS:
                        this.Bind(ViewModel, vm => vm.SelectedSource.id, v => v.txtId5.Text).DisposeWith(disposables);
                        this.Bind(ViewModel, vm => vm.SelectedSource.flow, v => v.cmbFlow5.Text).DisposeWith(disposables);
                        this.Bind(ViewModel, vm => vm.SelectedSource.security, v => v.txtSecurity5.Text).DisposeWith(disposables);
                        break;
                    case EConfigType.Trojan:
                        this.Bind(ViewModel, vm => vm.SelectedSource.id, v => v.txtId6.Text).DisposeWith(disposables);
                        this.Bind(ViewModel, vm => vm.SelectedSource.flow, v => v.cmbFlow6.Text).DisposeWith(disposables);
                        break;
                }
                this.Bind(ViewModel, vm => vm.SelectedSource.network, v => v.cmbNetwork.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedSource.headerType, v => v.cmbHeaderType.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedSource.requestHost, v => v.txtRequestHost.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedSource.path, v => v.txtPath.Text).DisposeWith(disposables);

                this.Bind(ViewModel, vm => vm.SelectedSource.streamSecurity, v => v.cmbStreamSecurity.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedSource.sni, v => v.txtSNI.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedSource.allowInsecure, v => v.cmbAllowInsecure.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedSource.fingerprint, v => v.cmbFingerprint.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedSource.alpn, v => v.cmbAlpn.Text).DisposeWith(disposables);

                this.BindCommand(ViewModel, vm => vm.SaveCmd, v => v.btnSave).DisposeWith(disposables);

            });

            this.Title = $"{profileItem.configType}";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtRemarks.Focus();
        }

        private void CmbNetwork_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetHeaderType();
            SetTips();
        }
        private void CmbStreamSecurity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var security = cmbStreamSecurity.SelectedItem.ToString();
            if (Utils.IsNullOrEmpty(security))
            {
                gridTlsMore.Visibility = Visibility.Hidden;
            }
            else
            {
                gridTlsMore.Visibility = Visibility.Visible;
            }
        }
        private void btnGUID_Click(object sender, RoutedEventArgs e)
        {
            txtId.Text =
            txtId5.Text = Utils.GetGUID();
        }

        private void SetHeaderType()
        {
            cmbHeaderType.Items.Clear();

            var network = cmbNetwork.SelectedItem.ToString();
            if (Utils.IsNullOrEmpty(network))
            {
                cmbHeaderType.Items.Add(Global.None);
                return;
            }

            if (network.Equals(Global.DefaultNetwork))
            {
                cmbHeaderType.Items.Add(Global.None);
                cmbHeaderType.Items.Add(Global.TcpHeaderHttp);
            }
            else if (network.Equals("kcp") || network.Equals("quic"))
            {
                cmbHeaderType.Items.Add(Global.None);
                Global.kcpHeaderTypes.ForEach(it =>
                {
                    cmbHeaderType.Items.Add(it);
                });
            }
            else if (network.Equals("grpc"))
            {
                cmbHeaderType.Items.Add(Global.GrpcgunMode);
                cmbHeaderType.Items.Add(Global.GrpcmultiMode);
            }
            else
            {
                cmbHeaderType.Items.Add(Global.None);
            }
            cmbHeaderType.SelectedIndex = 0;
        }

        private void SetTips()
        {
            var network = cmbNetwork.SelectedItem.ToString();
            if (Utils.IsNullOrEmpty(network))
            {
                network = Global.DefaultNetwork;
            }
            labHeaderType.Visibility = Visibility.Visible;
            tipRequestHost.Text =
            tipPath.Text =
            tipHeaderType.Text = string.Empty;

            if (network.Equals(Global.DefaultNetwork))
            {
                tipRequestHost.Text = ResUI.TransportRequestHostTip1;
                tipHeaderType.Text = ResUI.TransportHeaderTypeTip1;
            }
            else if (network.Equals("kcp"))
            {
                tipHeaderType.Text = ResUI.TransportHeaderTypeTip2;
                tipPath.Text = ResUI.TransportPathTip5;
            }
            else if (network.Equals("ws"))
            {
                tipRequestHost.Text = ResUI.TransportRequestHostTip2;
                tipPath.Text = ResUI.TransportPathTip1;
            }
            else if (network.Equals("h2"))
            {
                tipRequestHost.Text = ResUI.TransportRequestHostTip3;
                tipPath.Text = ResUI.TransportPathTip2;
            }
            else if (network.Equals("quic"))
            {
                tipRequestHost.Text = ResUI.TransportRequestHostTip4;
                tipPath.Text = ResUI.TransportPathTip3;
                tipHeaderType.Text = ResUI.TransportHeaderTypeTip3;
            }
            else if (network.Equals("grpc"))
            {
                tipPath.Text = ResUI.TransportPathTip4;
                tipHeaderType.Text = ResUI.TransportHeaderTypeTip4;
                labHeaderType.Visibility = Visibility.Hidden;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
