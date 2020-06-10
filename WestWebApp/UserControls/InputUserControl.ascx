<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InputUserControl.ascx.cs" Inherits="WaterSimUI.UserControls.InputUserControl" %>

<div id="ControlContainer" runat="server" class="InputControl" data-key="" data-fld="" data-Subs="">
    <asp:Label ID="lblSliderfldName" runat="server" class="field-name"></asp:Label> : <asp:Label runat="server" ID="lblSliderVal"></asp:Label> 
 <%-- <asp:Label ID="lblSliderfldName" runat="server"></asp:Label> --%>
 <%--       <asp:Label ID="lblunits" runat="server" Text="%"></asp:Label>--%>
        <asp:Label ID="lblunits" runat="server" Text=""></asp:Label>
    <div runat="server" id="containerHelp" class="help">
        <asp:HiddenField ID="hvHelpURI" runat="server" />
       <%-- <img src="../Images/icon_help.png" />--%></div>
    <asp:Label ID="lblSliderKeyWord" runat="server"></asp:Label>
    <p><span class="icon-close-open"></span></p>
    <div class="slider-container">
        <div runat="server" class="ui-slider-popup-button" id="PopupButton"></div>
        <div runat="server" id="divslider" class="InputSliderControl" data-min="" data-max="" data-def="" ></div>
        <div class="scale">
              <!-- DAS 10.14.14 -->
            <span id="lblScalept1" runat="server" style="left: 0%">0</span>            
            <span id="lblScalept2" runat="server" style="left: 25%">25</span>
            <span id="lblScalept3" runat="server" style="left: 50%">50</span>
            <span id="lblScalept4" runat="server" style="left: 75%">75</span>
            <span id="lblScalept5" runat="server" style="left: 100%">100</span>
        </div>
    </div>
</div>

<!-- Effluent popup modal subcontrols -->
<div id="effluent-popup-subcontrols" title="Effluent Sub Controls">
</div>
