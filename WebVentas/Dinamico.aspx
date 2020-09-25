<%@ Page Title="Dinamico" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dinamico.aspx.cs" Inherits="WebVentas.Dinamico" %>

<%@ Register assembly="DevExpress.Web.ASPxPivotGrid.v19.2, Version=19.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPivotGrid" tagprefix="dx" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <br />
    <table style="width:100%">
        <tr>
            <td style="padding-right: 8px;">
                <dx:ASPxPivotCustomizationControl ID="ASPxPivotCustomizationControl1" 
                    ClientInstanceName="ASPxPivotCustomizationControl1"
                    runat="server" Layout="BottomPanelOnly2by2" 
                    AllowedLayouts="BottomPanelOnly1by4, BottomPanelOnly2by2, StackedDefault, StackedSideBySide"
                    AllowSort="true" AllowFilter="true" ASPxPivotGridID="ASPxPivotGrid1" Theme="Office365" />
            </td>
            <td>
                <dx:ASPxPivotGrid ID="ASPxPivotGrid1" ClientInstanceName="ASPxPivotGrid1" runat="server" 
                    OptionsData-DataProcessingEngine="Optimized" CssClass="dxpgControl" Width="100%"
                     OptionsCustomization-CustomizationFormStyle="Excel2007" 
                    Height="500px" ClientIDMode="AutoID" 
                    DataSourceID="SqlDataSourceDB" EnableTheming="True" Theme="Office365" >
                <Styles>
                    <FieldValueStyle Wrap="False" />
                </Styles>
                <Fields>
                                    
                   
                    <dx:PivotGridField Area="RowArea" AreaIndex="0" Caption="ARTICULO" FieldName="ARTICULO_NOMBRE"
                        ID="fieldESTADO" />
                   
                    <dx:PivotGridField Area="ColumnArea" AreaIndex="0" Caption="DIA" FieldName="FECHA_DOCU"
                        ID="PivotGridField7" GroupInterval="DateDay" />
                                   

                    <dx:PivotGridField Area="DataArea" AreaIndex="0" Caption="IMPORTE" FieldName="IMPORTE"
                        ID="fieldUNIDADES" CellFormat-FormatString="N" CellFormat-FormatType="Numeric"/>
                                    

                    <dx:PivotGridField Area="FilterArea" AreaIndex="0" Caption="AÑO" FieldName="FECHA_DOCU"
                        ID="PivotGridField1" GroupInterval="DateYear" />
                    <dx:PivotGridField Area="FilterArea" AreaIndex="1" Caption="MES" FieldName="FECHA_DOCU"
                        ID="PivotGridField2" GroupInterval="DateMonth"  />
                  
                   
                    <dx:PivotGridField Area="FilterArea" AreaIndex="3" Caption="TIPO" FieldName="CLIENTE_TIPO"
                        ID="PivotGridFieldNumOrden" />
                    <dx:PivotGridField Area="FilterArea" AreaIndex="4" Caption="DOCUMENTO" FieldName="CLIENTE_DOCUMENTO"
                        ID="PivotGridFieldSerialNumber" />
                    <dx:PivotGridField Area="FilterArea" AreaIndex="5" Caption="CLIENTE" FieldName="CLIENTE_NOMBRE"
                        ID="PivotGridFieldProducto" />
                    <dx:PivotGridField Area="FilterArea" AreaIndex="6" Caption="DISTRITO" FieldName="CLIENTE_DISTRITO"
                        ID="PivotGridFieldCuenta" />
                    <dx:PivotGridField Area="FilterArea" AreaIndex="7" Caption="CODIGO" FieldName="ARTICULO_CODIGO"
                        ID="PivotGridARTICULO_CODIGO" />
                    <dx:PivotGridField Area="FilterArea" AreaIndex="8" Caption="CANTIDAD" FieldName="ARTICULO_CANTIDAD"
                        ID="PivotGridARTICULO_CANTIDAD" CellFormat-FormatString="n0" CellFormat-FormatType="Numeric" />

                </Fields>
                <OptionsPager Visible="false"/>
                <OptionsView HorizontalScrollBarMode="Auto" VerticalScrollBarMode="Auto" RowTotalsLocation="Tree" 
                    ShowTotalsForSingleValues="True"
                    ShowColumnHeaders="False" ShowDataHeaders="False" ShowFilterHeaders="False" ShowRowHeaders="False" />
                <OptionsCustomization CustomizationFormStyle="Excel2007"/>
                <ClientSideEvents Init="function(s, e) {
                                if(typeof(ASPxPivotCustomizationControl1) == 'undefined')
                                    return;
                                setTimeout( 
                                    function(){
                                        if(ASPxPivotGrid1.GetMainDiv().clientHeight &gt; 400)
                                        ASPxPivotCustomizationControl1.SetHeight(ASPxPivotGrid1.GetMainElement().offsetHeight); 
                                        else 
                                        ASPxPivotCustomizationControl1.SetHeight(400);
                                    }, 100);
                                }" />
                </dx:ASPxPivotGrid>
            </td>
        </tr>
     </table>
       <asp:SqlDataSource ID="SqlDataSourceDB"  runat="server" 
           ConnectionString="<%$ ConnectionStrings:dbreporte %>"> 
            <%--SelectCommand="SELECT * FROM [vDOCUMENTO_REPORTE_DETALLE]  where EMPRESA_RUC='@ruc'">
          <selectparameters>
              <asp:QueryStringParameter name="ruc" Type="String" QueryStringField="qruc" />
          </selectparameters>--%>
       </asp:SqlDataSource>

        <dx:ASPxPivotGridExporter ID="ASPxPivotGridExporter1" runat="server" ASPxPivotGridID="ASPxPivotGrid1" Visible="False" />

        <dx:ASPxFormLayout runat="server" ID="OptionsFormLayout" ClientInstanceName="optionsFormLayout">
            <Items>
                <dx:LayoutGroup Caption="Export Settings">
                    <Items>
                        <dx:LayoutItem Caption="Export Mode">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxComboBox ID="listExportFormat" runat="server" Style="vertical-align: middle" SelectedIndex="6"
                                        ValueType="System.String" Width="220px" >
                                        <Items>
                                            <dx:ListEditItem Text="Pdf" Value="0" />
                                            <dx:ListEditItem Text="Excel" Value="1" />
                                            <dx:ListEditItem Text="Mht" Value="2" />
                                            <dx:ListEditItem Text="Rtf" Value="3" />
                                            <dx:ListEditItem Text="Text" Value="4" />
                                            <dx:ListEditItem Text="Html" Value="5" />
                                            <dx:ListEditItem Text="Excel (Data Aware)" Value="6" />
                                        </Items>
                                        <ClientSideEvents
                                            Init="function(s, e) {
                                            var fieldHeaderOptionsPanel = optionsFormLayout.GetItemByName('fieldHeaderOptionsPanel');
                                            fieldHeaderOptionsPanel.SetVisible(false);
                                            var fieldValuesOptionsPanel = optionsFormLayout.GetItemByName('fieldValuesOptionsPanel');
                                            fieldValuesOptionsPanel.SetVisible(false);
                                            var dataAwareOptionsPanel = optionsFormLayout.GetItemByName('dataAwareOptionsPanel');
                                            dataAwareOptionsPanel.SetVisible(true);
                                            checkCustomFormattedValuesAsText.SetEnabled(false);
                                        }"
                                            SelectedIndexChanged="function(s, e) {
                                            var selectedIndex = s.GetSelectedIndex(),
                                                isExportToExcel = selectedIndex == 1,
                                                isDataAwareExport = selectedIndex == 6;
                                            var fieldHeaderOptionsPanel = optionsFormLayout.GetItemByName('fieldHeaderOptionsPanel');
                                            fieldHeaderOptionsPanel.SetVisible(!isDataAwareExport);
                                            var fieldValuesOptionsPanel = optionsFormLayout.GetItemByName('fieldValuesOptionsPanel');
                                            fieldValuesOptionsPanel.SetVisible(!isDataAwareExport);
                                            var dataAwareOptionsPanel = optionsFormLayout.GetItemByName('dataAwareOptionsPanel');
                                            dataAwareOptionsPanel.SetVisible(isDataAwareExport);
                                            checkCustomFormattedValuesAsText.SetEnabled(isExportToExcel);
                                        }" />
                                    </dx:ASPxComboBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxButton ID="ASPxButton3" ClientInstanceName="buttonSaveAs" runat="server" ToolTip="Export and Save"
                                        OnClick="buttonSaveAs_Click" Text="Export" Width="220px" />
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:LayoutGroup>
                <dx:LayoutGroup Caption="Data Aware Options" Name="dataAwareOptionsPanel">
                    <Items>
                        <dx:LayoutItem Caption="Allow grouping columns/rows">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxCheckBox ID="allowGroupingCheckBox" ClientInstanceName="allowGroupingCheckBox" runat="server" Checked="True" />
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Allow fixed ColumnArea and RowArea">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxCheckBox ID="allowFixedColumns" ClientInstanceName="allowFixedColumns" runat="server" Checked="True" />
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Export cell values as display text">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxCheckBox ID="exportCellValuesAsText" ClientInstanceName="exportCellValuesAsText" runat="server" />
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Export raw data">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxCheckBox ID="exportRawData" ClientInstanceName="exportRawData" runat="server" CheckState="Unchecked" />
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:LayoutGroup>
                <dx:LayoutGroup Caption="Field Header Options" Name="fieldHeaderOptionsPanel" >
                    <Items>
                        <dx:LayoutItem Caption="Print filter field headers">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxCheckBox ID="checkPrintFilterFieldHeaders" runat="server" Checked="True" />
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Print column field headers">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxCheckBox ID="checkPrintColumnFieldHeaders" runat="server" Checked="True" />
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Print row field headers">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxCheckBox ID="checkPrintRowFieldHeaders" runat="server" Checked="True" />
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Print data field headers">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxCheckBox ID="checkPrintDataFieldHeaders" runat="server" Checked="True" />
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:LayoutGroup>
                <dx:LayoutGroup Caption="Field Values Options" Name="fieldValuesOptionsPanel">
                    <Items>
                        <dx:LayoutItem Caption="Print column area on every page">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxCheckBox ID="checkPrintColumnAreaOnEveryPage" runat="server" />
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Print row area on every page">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxCheckBox ID="checkPrintRowAreaOnEveryPage" runat="server" />
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Merge values of outer column fields">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxCheckBox ID="checkMergeColumnFieldValues" runat="server" Checked="True" />
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Merge values of outer row fields">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxCheckBox ID="checkMergeRowFieldValues" runat="server" Checked="True" />
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Export custom formatted values as text">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxCheckBox ID="checkCustomFormattedValuesAsText" ClientInstanceName="checkCustomFormattedValuesAsText" runat="server" Checked="True" />
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:LayoutGroup>
            </Items>
        </dx:ASPxFormLayout>
    
</asp:Content>
