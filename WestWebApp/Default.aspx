<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WaterSimUI._Default" %>

<%@ Register Src="UserControls/OutputUserControl.ascx" TagName="OutputUserControl" TagPrefix="Wsmo" %>

<%@ Register Src="UserControls/InputUserControl.ascx" TagName="InputUserControl" TagPrefix="Wsmi" %>


<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="GraphControls">
    <div id="main-input-container">
        <!-- Nav tabs -->
        <ul class="nav nav-tabs" role="tablist">
            <li class="nav-item">
                <a class="nav-link custom-nav-link active" data-toggle="tab" id="policyPane-tab" name="#policyPane" role="tab">Policy</a>
            </li>
            <li class="nav-item">
                <a class="nav-link custom-nav-link" data-toggle="tab" id="draughtPane-tab" name="#draughtPane" role="tab">Drought</a>
            </li>
        </ul>

        <!-- Tab panes -->
        <div class="tab-content">
            <div id="policyPane" class="tab-pane input-pane active" role="tabpanel" style="position: relative;">
                <div class="chart-controls" id="flow-controls">
                    <div>
                        <div id="policy-source-controls" style="float: left;">
                            <span class="policy-title">Water Source Availability</span>
                        </div>
                        <%--<div id="policy-source-controls-amount" style="float: left; height: 100%; width: 100px; background-color: #E3F3F7; margin-top: 56px;">

                        </div>--%>
                        <div style="clear: both;"></div>
                    </div>
                    <div>
                        <div id="policy-consumer-controls" style="float: left;">
                            <span class="policy-title">Change in Water Use Efficiency for:</span>
                        </div>
                        <%--<div id="policy-consumer-controls-amount" style="float: left; height: 100%; width: 100px; background-color: #E3F3F7; padding-top: 56px;">

                        </div>--%>
                        <div style="clear: both;"></div>
                    </div>
                    <%--<div style="display: block; width:100%; border-bottom: 1px solid black;margin-top: 12px;"></div>--%>
                    <div>
                        <div id="policy-pop-controls" style="float: left;">
                        </div>
                        <div style="clear: both;"></div>
                    </div>

                </div>
            </div>
            <div id="draughtPane" class="tab-pane input-pane" role="tabpanel" style="margin-bottom: 25px;">
                <div id="climate-controls" style="float: left;">
                    <span class="policy-title">Drought Distribution Options</span>
                    <%--<div id="DSCNInputUserControl_controlgroup" class="realclearfix controlgroup" style="display: block;">
                        <span id="DSCNInputUserControl_lblSliderfldName" class="field-name">Drought Severity</span>
                        <img class="info-icon info-item" src="Images/info_icon.png" data-fld="DSCN_P">
                        <br />
                        <div id="DSCNInputUserControl_buttonset" class="radio-container-climate" style="float: left;">
                            <input type="radio" id="DSCNInputUserControl_radio_0" name="DSCNInputUserControl_radio" value="4" /><label class="input-button-0" for="DSCNInputUserControl_radio_0">Extreme</label>
                            <input type="radio" id="DSCNInputUserControl_radio_1" name="DSCNInputUserControl_radio" value="3" /><label class="input-button-1" for="DSCNInputUserControl_radio_1">Severe</label>
                            <input type="radio" id="DSCNInputUserControl_radio_2" name="DSCNInputUserControl_radio" value="2" /><label class="input-button-2" for="DSCNInputUserControl_radio_2">No Change</label>
                            <input type="radio" id="DSCNInputUserControl_radio_3" name="DSCNInputUserControl_radio" value="1" /><label class="input-button-3" for="DSCNInputUserControl_radio_3">Slight</label>
                            <input type="radio" id="DSCNInputUserControl_radio_4" name="DSCNInputUserControl_radio" value="0" checked="checked"/><label class="input-button-4" for="DSCNInputUserControl_radio_4">None</label>
                        </div>
                    </div>--%>
                    <%--<div id="CLIMInputUserControl_controlgroup" class="realclearfix controlgroup" style="display: block;">
                        <span id="CLIMInputUserControl_lblSliderfldName" class="field-name">Drought Impacts on Rivers/Lakes</span>
                        <br />
                        <div id="CLIMInputUserControl_buttonset" class="radio-container-climate" style="float: left;">
                            <input type="radio" id="CLIMInputUserControl_radio_5" name="CLIMInputUserControl_radio" value="5" /><label class="input-button-0" for="CLIMInputUserControl_radio_5">Flood</label>

                            <input type="radio" id="CLIMInputUserControl_radio_0" name="CLIMInputUserControl_radio" value="0" checked="checked" /><label class="input-button-1" for="CLIMInputUserControl_radio_0">No Effect</label>
                            <input type="radio" id="CLIMInputUserControl_radio_1" name="CLIMInputUserControl_radio" value="1" /><label class="input-button-2" for="CLIMInputUserControl_radio_1">Slight</label>
                            <input type="radio" id="CLIMInputUserControl_radio_3" name="CLIMInputUserControl_radio" value="3" /><label class="input-button-3" for="CLIMInputUserControl_radio_3">Moderate</label>
                            <input type="radio" id="CLIMInputUserControl_radio_4" name="CLIMInputUserControl_radio" value="4" /><label class="input-button-4" for="CLIMInputUserControl_radio_4">Severe</label>
                        </div>
                    </div>
                    <div id="DCInputUserControl_controlgroup" class="realclearfix controlgroup" style="display: block;">
                        <span id="DCInputUserControl_lblSliderfldName" class="field-name">Drought Impacts Rate</span>
                        <br />
                        <div id="DCInputUserControl_buttonset" class="radio-container-climate" style="float: left;">
                            <input type="radio" id="DCInputUserControl_radio_5" name="DCInputUserControl_radio" value="5" /><label class="input-button-0" for="DCInputUserControl_radio_5">Flood</label>
                            <input type="radio" id="DCInputUserControl_radio_0" name="DCInputUserControl_radio" value="0" checked="checked" /><label class="input-button-1" for="DCInputUserControl_radio_0">No Effect</label>
                            <input type="radio" id="DCInputUserControl_radio_1" name="DCInputUserControl_radio" value="1" /><label class="input-button-2" for="DCInputUserControl_radio_1">Slight</label>
                            <input type="radio" id="DCInputUserControl_radio_3" name="DCInputUserControl_radio" value="3" /><label class="input-button-3" for="DCInputUserControl_radio_3">Moderate</label>
                            <input type="radio" id="DCInputUserControl_radio_4" name="DCInputUserControl_radio" value="4" /><label class="input-button-4" for="DCInputUserControl_radio_4">Severe</label>
                        </div>
                    </div>--%>
                </div>
                <div style="clear: both;"></div>
            </div>
        </div>
    </div>
    <div id="main-output-container" style="float: left;">
        <!-- Nav tabs -->
        <ul class="nav nav-tabs" role="tablist">
            <li class="nav-item">
                <a class="nav-link custom-nav-link active" data-toggle="tab" id="flowChart-tab" name="#flowChart" role="tab">Flow Chart</a>
            </li>
            <li class="nav-item">
                <a class="nav-link custom-nav-link" data-toggle="tab" id="barCharts-tab" name="#barCharts" role="tab">Bar Charts</a>
            </li>
            <li class="nav-item">
                <a class="nav-link custom-nav-link" data-toggle="tab" id="lineCharts-tab" name="#lineCharts" role="tab">Line Charts</a>
            </li>
        </ul>

        <!-- Tab panes -->
        <div class="tab-content" style="float: left;">
            <div class="tab-pane active" id="flowChart" role="tabpanel" style="position: relative;">
                <%--<div class="chart-controls" id="flow-controls" style="float: left; margin-right: 20px;">
                    <span class="policy-title">Water Distribution Options</span>
                </div>--%>
                <div class="chart" style="display: inline-block; height: 500px; margin-top: 33px;">
                    <Wsmo:OutputUserControl runat="server" ID="OutputUserControl3" Type="WSASK" FieldName="GW_P,REC_P,SUR_P,SURL_P,SAL_P,UD_P,AD_P,ID_P,PD_P,UDN_P,ADN_P,IDN_P,PDN_P,SUR_UD_P,SUR_AD_P,SUR_ID_P,SUR_PD_P,SURL_UD_P,SURL_AD_P,SURL_ID_P,SURL_PD_P,GW_UD_P,GW_AD_P,GW_ID_P,GW_PD_P,SAL_UD_P,SAL_AD_P,SAL_ID_P,SAL_PD_P,REC_UD_P,REC_AD_P,REC_ID_P,REC_PD_P" Title="Sandkey" SeriesColors="5" />
                </div>
                <div style="clear: both;"></div>
            </div>
            <div class="tab-pane realclearfix" id="barCharts" role="tabpanel">
                <%--<div class="chart-controls" id="chart-controls" style="float: left; margin-right: 20px;">
                    <span class="policy-title">Water Distribution Options</span>
                </div>--%>
                <div id="barChart-container" style="float: left;">
                    <div class="chart" style="display: inline-block; margin-right: 5px; margin-top: 35px;">
                        <Wsmo:OutputUserControl runat="server" ID="OutputUserControl5" Type="WSASC" FieldName="UD_P,AD_P,ID_P,PD_P,UDN_P,ADN_P,IDN_P,PDN_P" Title="Consumers Total Demand" SeriesColors="5" />
                    </div>
                    <div class="chart" style="display: inline-block; margin-top: 35px;">
                        <Wsmo:OutputUserControl runat="server" ID="OutputUserControl6" Type="WSASF" FieldName="SUR_UD_P,SUR_AD_P,SUR_ID_P,SUR_PD_P,SURL_UD_P,SURL_AD_P,SURL_ID_P,SURL_PD_P,GW_UD_P,GW_AD_P,GW_ID_P,GW_PD_P,SAL_UD_P,SAL_AD_P,SAL_ID_P,SAL_PD_P,REC_UD_P,REC_AD_P,REC_ID_P,REC_PD_P" Title="Consumers" SeriesColors="5" />
                    </div>
                </div>
                <div style="clear: both;"></div>
            </div>
            <div class="tab-pane realclearfix" id="lineCharts" role="tabpanel">
                <%--<div class="chart-controls" id="line-chart-controls" style="float: left; margin-right: 20px;">
                    <span class="policy-title">Water Distribution Options</span>
                </div>--%>
                <div id="lineChart-container" style="float: left;">
                    <div class="chart" style="display: inline-block; margin-right: 5px; margin-top: 35px;">
                        <Wsmo:OutputUserControl runat="server" ID="OutputUserControl7" Type="WSASL" FieldName="UD_P,AD_P,ID_P,PD_P,UDN_P,ADN_P,IDN_P,PDN_P" Title="Consumers Total Demand" SeriesColors="5" />
                    </div>
                </div>
                <div style="clear: both;"></div>
            </div>
        </div>
        <div class="assessment">
            <div class="assessment-header">
                <%--<div class="assessment-title-space"></div>--%>
                <span class="assessment-title">Assessment: </span><span class="assessment-status" data-fld="SAI_P">Uh oh!</span>
                <div class="assessment-line-break"></div>
                <div class="assessment-title-space"></div>
                <span class="assessment-indicator-prefix">Supplies and demands are</span>
                <span class="assessment-indicator-prefix" data-fld="RNDR_P">well balanced.</span>
                <br />
                <span class="assessment-indicator-prefix">The regions' sustainability is</span>
                <span class="assessment-indicator-prefix" data-fld="ASI_P">very high!</span>
                <div style="clear: both"></div>
            </div>

            <!-- Assessment indicators are inserted below -->
        </div>

        <div style="clear: both;"></div>
    </div>
    <div style="clear: both;"></div>

    <script src="Scripts/Custom/load-files.js"></script>

</asp:Content>


