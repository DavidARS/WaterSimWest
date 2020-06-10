<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OutputUserControl.ascx.cs" Inherits="WaterSimUI.UserControls.OutputUserControl" %>

<div runat="server" style="position:relative; top:0px;" id="OutputControl" class="OutputControl" data-Type="" data-fld="" data-title="" data-series="">
    <select class="ddlType" id="ddlTypes" style="position:absolute; top:10px; left:0px; width:100px; height:30px; border:groove;">
        <option value="pie">Pie</option>
        <option value="column">Column</option>
    </select>
    <select class="ddlflds" id="ddlfld" style="position:absolute; top:10px; width:150px; right:0px; height:30px; border:groove;">
    </select>
    <%--<div runat="server" id="ChartContainer" style=" position:relative; top:20px; width:350px; height:480px;"></div>--%>
    <div runat="server" id="ChartContainer" style="position:relative; width:350px; height:480px;"></div>
    <asp:Label ID="lblChartOption" runat="server"></asp:Label>
    <asp:Label ID="lblFldName" runat="server"></asp:Label>
    <asp:Label ID="lblTitle" runat="server"></asp:Label>
    <asp:Label ID="lblSeriesColors" runat="server"></asp:Label>
</div>