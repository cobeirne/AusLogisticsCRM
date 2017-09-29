<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberView.aspx.cs" Inherits="AusLogisticsWebsite.Views.Default" Theme="AusLogisticsTheme" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AL | Member Management</title>
</head>
<body>
    <header>
        <img class="logo" src="../Images/AusLogisticsLogoBlueOnWhite.png" />
        <div class="company-name">Australian Logistics CRM</div>
    </header>
    <form runat="server">
        <section>
            <h1>Member Management</h1>

            <table>
                <tr>
                    <td><asp:LinkButton ID="lbtnViewMembers" runat="server" OnClick="lbtnViewMembers_Click">View Members</asp:LinkButton> |</td>
                    <td><asp:LinkButton ID="lbtnMigrateMember" runat="server" OnClick="lbtnMigrateMember_Click">Migrate Member</asp:LinkButton> |</td>
                    <td><asp:LinkButton ID="lbtnAddMember" runat="server" OnClick="lbtnAddMember_Click" >Add Member</asp:LinkButton> |</td>
                    <td><asp:LinkButton ID="lbtnRemoveMember" runat="server" OnClick="lbtnRemoveMember_Click">Remove Member</asp:LinkButton> |</td>
                    <td><asp:LinkButton ID="lbtnUpdateMember" runat="server" OnClick="lbtnUpdateMember_Click" >Update Member</asp:LinkButton> |</td>
                    <td><asp:LinkButton ID="lbtnReports" runat="server" OnClick="lbtnReports_Click">Reports</asp:LinkButton></td>
                </tr>
            </table>

            <asp:Panel ID="pnlViewMembers" runat="server" Visible="true">
                <article>
                    <h2>View Members</h2>

                    <table style="margin-left:auto; margin-right:auto;">
                        <tr>
                            <td>Member ID:</td>
                            <td><asp:TextBox ID="tbFindId" runat="server" Size="10" Height="20px"/></td>
                            <td>First Name:</td>
                            <td><asp:TextBox ID="tbFindFirstName" runat="server" /></td>
                            <td>Last Name:</td>
                            <td><asp:TextBox ID="tbFindLastName" runat="server" /></td>
                            <td>
                                <asp:Button ID="btnSearch" Text="Search" runat="server" OnClick="btnSearch_Click" ValidationGroup="ViewMembers" />

                                <div class="val-error">
                                    <asp:RegularExpressionValidator id="valViewMemberId" runat="server"
                                        ControlToValidate="tbFindId"
                                        ValidationExpression="^[0-9]+$"
                                        Text="Invalid Member ID"
                                        Display="Dynamic"
                                        forecolor="red"
                                        ValidationGroup="ViewMembers">
                                    </asp:RegularExpressionValidator>  
                                </div>
                            </td>
                        </tr>
                    </table>
                    <br />

                    <asp:DataGrid id="dgViewMembers" runat="server"
                        BorderColor="black"
                        BorderWidth="1"
                        CellPadding="3"
                        AllowPaging="true"
                        PageSize="25"
                        AutoGenerateColumns="false"        
                        OnPageIndexChanged="dgViewMembers_PageIndexChanged" HorizontalAlign="Center">

                      <HeaderStyle BackColor="#00aaaa" />

                      <PagerStyle Mode="NextPrev" />

                      <Columns>
                         <asp:BoundColumn 
                              HeaderText="Member ID" 
                              DataField="MembershipId"/>

                        <asp:BoundColumn 
                              HeaderText="Last Name" 
                              DataField="LastName"/>

                         <asp:BoundColumn 
                              HeaderText="First Name" 
                              DataField="FirstName"/>

                        <asp:BoundColumn 
                              HeaderText="Membership Class" 
                              DataField="MemberClass"/>

                        <asp:BoundColumn 
                              HeaderText="Date Joined" 
                              DataField="DateJoined"
                              DataFormatString="{0:d}" />

                        <asp:BoundColumn 
                              HeaderText="Date of Birth" 
                              DataField="DateOfBirth"
                              DataFormatString="{0:d}" />

                        <asp:BoundColumn 
                              HeaderText="Salary"
                              DataField="Salary"
                              DataFormatString="{0:C}" />

                        <asp:BoundColumn 
                              HeaderText="Gender" 
                              DataField="Gender">
                            <ItemStyle HorizontalAlign="Center"/>
                        </asp:BoundColumn>

                      </Columns>
                   </asp:DataGrid>

                </article>  
            </asp:Panel>

            <asp:Panel ID="pnlMigrateMembers" runat="server" Visible="false">
                <article>
                    <h2>Migrate Member</h2>

                    <table>
                        <tr>
                            <td class="form-label">Employee ID:</td>
                            <td><asp:TextBox ID="tbMigrateEmployeeNumber" runat="server" Size="10"/></td>
                            <td>
                                <asp:Button ID="btnMigrateSearch" Text="Search" runat="server" OnClick="btnMigrateSearch_Click" ValidationGroup="MigrateEmployee" />

                                <div class="val-error">
                                    <asp:RegularExpressionValidator id="valMigrateEmployeeId" runat="server"
                                        ControlToValidate="tbMigrateEmployeeNumber"
                                        ValidationExpression="^[0-9]+$"
                                        Text="Invalid Employee ID"
                                        Display="Dynamic"
                                        forecolor="red"
                                        ValidationGroup="MigrateEmployee">
                                    </asp:RegularExpressionValidator>  
                                </div>
                            </td>
                        </tr>
                    </table>
                    <br />

                    <table>
                        <tr>
                            <td class="form-label">First Name:</td>
                            <td>
                                <asp:Label ID="lblMigrateFirstName" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="form-label">Last Name:</td>
                            <td>
                                <asp:Label ID="lblMigrateLastName" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="form-label">Date of Birth:</td>
                            <td>
                                <asp:Label ID="lblMigrateDateOfBirth" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="form-label">Gender:</td>
                            <td>
                                <asp:Label ID="lblMigrateGender" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                           <td><br /><asp:Button id="btnMigrateMember" Text="Migrate Member" runat="server" OnClick="btnMigrateMember_Click" ValidationGroup="AddMembers" Enabled="False" /></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td><asp:Label ID="lblMigrateResult" runat="server" /></td>
                        </tr>

                    </table>


                </article>  
            </asp:Panel>

            <asp:Panel ID="pnlAddMember" runat="server" Visible="false">
                <article>
                    <h2>Add Member</h2>

                    <table>
                        <tr>
                            <td class="form-label">First Name:</td>
                            <td>
                                <asp:TextBox id="tbFirstName" runat="server" size="34" maxlength="34"/>
                                <div class="val-error">
                                    <asp:RegularExpressionValidator id="valFirstName" runat="server" 
                                        ControlToValidate="tbFirstName" 
                                        ErrorMessage="Invalid First Name" 
                                        ValidationExpression="^[a-zA-Z-]*$"
                                        forecolor="red"
                                        Display="Dynamic"
                                        ValidationGroup="AddMembers">
                                    </asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="valFirstNameReq" runat="server"
                                        ControlToValidate="tbFirstName"
                                        ErrorMessage="Required Field"
                                        forecolor="red"
                                        Display="Dynamic"
                                        ValidationGroup="AddMembers">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td />
                            <td><div class="input-hint">e.g. John or Mary-Jane</div><br /></td>
                        </tr>
                        <tr>
                            <td class="form-label">Last Name:</td>
                            <td>
                                <asp:TextBox id="tbLastName" size="34" maxlength="34" runat="server" />
                                <div class="val-error">
                                    <asp:RegularExpressionValidator id="valLastName" runat="server" 
                                        ControlToValidate="tbLastName" 
                                        ErrorMessage="Invalid Last Name" 
                                        ValidationExpression="^[a-zA-Z-']*$"
                                        Display="Dynamic"
                                        forecolor="red"
                                        ValidationGroup="AddMembers">
                                    </asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="valLastNameReq" runat="server"
                                        ControlToValidate="tbLastName"
                                        ErrorMessage="Required Field"
                                        forecolor="red"
                                        Display="Dynamic"
                                        ValidationGroup="AddMembers">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td />
                            <td><div class="input-hint">e.g. White or Smith-Jones</div><br /></td>
                        </tr>
                        <tr>
                            <td class="form-label">Gender:</td>
                            <td>
                                <asp:DropDownList id="ddGender" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp</td>
                            <td>&nbsp</td>
                        </tr>
                        <tr>
                            <td class="form-label">Salary:</td>
                            <td>
                                <div class="input-unit">$</div><asp:TextBox id="tbSalary" size="7" maxlength="7" runat="server" />
                                <div class="val-error">
                                    <asp:RangeValidator id="valSalary" runat="server"
                                        ControlToValidate="tbSalary"
                                        MinimumValue="0"
                                        MaximumValue="9999999"
                                        Type="Integer"
                                        Text="Invalid salary"
                                        Display="Dynamic"
                                        forecolor="red"
                                        ValidationGroup="AddMembers">
                                    </asp:RangeValidator>  
                                    <asp:RequiredFieldValidator ID="valSalaryReq" runat="server"
                                        ControlToValidate="tbSalary"
                                        ErrorMessage="Required Field"
                                        forecolor="red"
                                        Display="Dynamic"
                                        ValidationGroup="AddMembers">
                                    </asp:RequiredFieldValidator>                                                                    
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td />
                            <td><div class="input-hint">Min $0 to Max $9999999 (Round up to nearest whole number)</div><br /></td>
                        </tr>
                        <tr>
                            <td class="form-label">Date of Birth:</td>
                            <td>
                                <asp:Calendar ID="calBirthday" SelectionMode="Day" ShowGridLines="True" runat="server"></asp:Calendar><br />
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                           <td><br /><asp:Button id="btAddSubmit" Text="Add Member" runat="server" OnClick="btAddSubmit_Click" ValidationGroup="AddMembers" /></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td><asp:Label ID="lblAddMemberResult" runat="server" /></td>
                        </tr>
                    </table>

                </article>  
            </asp:Panel>

            <asp:Panel ID="pnlRemoveMember" runat="server" Visible="false">
                <article>
                    <h2>Remove Member</h2>

                    <table>
                        <tr>
                            <td class="form-label">Membership ID:</td>
                            <td><asp:TextBox ID="tbRemoveMembershipId" runat="server" Size="10" /></td>
                            <td>
                                <asp:Button ID="btnRemoveMemberSearch" Text="Search" runat="server" OnClick="btnRemoveMemberSearch_Click" ValidationGroup="RemoveMembers" />

                                <div class="val-error">
                                    <asp:RegularExpressionValidator id="valRemoveMembershipId" runat="server"
                                        ControlToValidate="tbRemoveMembershipId"
                                        ValidationExpression="^[0-9]+$"
                                        Text="Invalid Member ID"
                                        Display="Dynamic"
                                        forecolor="red"
                                        ValidationGroup="RemoveMembers">
                                    </asp:RegularExpressionValidator>  
                                </div>

                            </td>
                        </tr>
                    </table>
                    <br />

                    <table>
                        <tr>
                            <td class="form-label">First Name:</td>
                            <td>
                                <asp:Label ID="lblRemoveFirstName" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="form-label">Last Name:</td>
                            <td>
                                <asp:Label ID="lblRemoveLastName" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="form-label">Date of Birth:</td>
                            <td>
                                <asp:Label ID="lblRemoveDateOfBirth" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="form-label">Gender:</td>
                            <td>
                                <asp:Label ID="lblRemoveGender" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                           <td><br /><asp:Button id="btnRemoveMember" Text="Remove Member" runat="server" OnClick="btnRemoveMember_Click" ValidationGroup="AddMembers" Enabled="false" /></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td><asp:Label ID="lblRemoveResult" runat="server" /></td>
                        </tr>

                    </table>

                </article>  
            </asp:Panel>

            <asp:Panel ID="pnlUpdateMember" runat="server" Visible="false">
                <article>
                    <h2>Update Member</h2>

                    <table>
                        <tr>
                            <td class="form-label">Membership ID:</td>
                            <td><asp:TextBox ID="tbUpdateMemberId" runat="server" Size="10"/></td>
                            <td>
                                <asp:Button ID="btnUpdateSearch" Text="Search" runat="server" OnClick="btnUpdateSearch_Click" ValidationGroup="UpdateSearch" />

                                <div class="val-error">
                                    <asp:RegularExpressionValidator id="valUpdateMemberId" runat="server"
                                        ControlToValidate="tbUpdateMemberId"
                                        ValidationExpression="^[0-9]+$"
                                        Text="Invalid Member ID"
                                        Display="Dynamic"
                                        forecolor="red"
                                        ValidationGroup="UpdateSearch">
                                    </asp:RegularExpressionValidator>  
                                </div>
                            </td>
                        </tr>
                    </table>
                    <br />

                    <table>
                        <tr>
                            <td class="form-label">Membership ID:</td>
                            <td>
                                <asp:Label ID="lblUpdateMembershipId" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp</td>
                            <td>&nbsp</td>
                        </tr>
                        <tr>
                            <td class="form-label">First Name:</td>
                            <td>
                                <asp:TextBox id="tbUpdateFirstName" runat="server" size="34" maxlength="34"/>
                                <div class="val-error">
                                    <asp:RegularExpressionValidator id="valUpdateFirstNameExpression" runat="server" 
                                        ControlToValidate="tbUpdateFirstName" 
                                        ErrorMessage="Invalid First Name" 
                                        ValidationExpression="^[a-zA-Z-]*$"
                                        forecolor="red"
                                        Display="Dynamic"
                                        ValidationGroup="UpdateMembers">
                                    </asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="valUpdateFirstNameRequired" runat="server"
                                        ControlToValidate="tbUpdateFirstName"
                                        ErrorMessage="Required Field"
                                        forecolor="red"
                                        Display="Dynamic"
                                        ValidationGroup="UpdateMembers">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td />
                            <td><div class="input-hint">e.g. John or Mary-Jane</div><br /></td>
                        </tr>
                        <tr>
                            <td class="form-label">Last Name:</td>
                            <td>
                                <asp:TextBox id="tbUpdateLastName" size="34" maxlength="34" runat="server" />
                                <div class="val-error">
                                    <asp:RegularExpressionValidator id="valUpdateLastNameExpression" runat="server" 
                                        ControlToValidate="tbUpdateLastName" 
                                        ErrorMessage="Invalid Last Name" 
                                        ValidationExpression="^[a-zA-Z-']*$"
                                        Display="Dynamic"
                                        forecolor="red"
                                        ValidationGroup="UpdateMembers">
                                    </asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="valUpdateLastNameRequired" runat="server"
                                        ControlToValidate="tbUpdateLastName"
                                        ErrorMessage="Required Field"
                                        forecolor="red"
                                        Display="Dynamic"
                                        ValidationGroup="UpdateMembers">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td />
                            <td><div class="input-hint">e.g. White or Smith-Jones</div><br /></td>
                        </tr>
                        <tr>
                            <td class="form-label">Gender:</td>
                            <td>
                                <asp:DropDownList id="ddUpdateGender" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp</td>
                            <td>&nbsp</td>
                        </tr>
                        <tr>
                            <td class="form-label">Salary:</td>
                            <td>
                                <div class="input-unit">$</div><asp:TextBox id="tbUpdateSalary" size="7" maxlength="7" runat="server" />
                                <div class="val-error">
                                    <asp:RangeValidator id="valUpdateSalaryExpression" runat="server"
                                        ControlToValidate="tbUpdateSalary"
                                        MinimumValue="0"
                                        MaximumValue="9999999"
                                        Type="Integer"
                                        Text="Invalid salary"
                                        Display="Dynamic"
                                        forecolor="red"
                                        ValidationGroup="UpdateMembers">
                                    </asp:RangeValidator>  
                                    <asp:RequiredFieldValidator ID="valUpdateSalaryRequired" runat="server"
                                        ControlToValidate="tbUpdateSalary"
                                        ErrorMessage="Required Field"
                                        forecolor="red"
                                        Display="Dynamic"
                                        ValidationGroup="UpdateMembers">
                                    </asp:RequiredFieldValidator>                                                                    
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td />
                            <td><div class="input-hint">Min $0 to Max $9999999 (Round up to nearest whole number)</div><br /></td>
                        </tr>
                        <tr>
                            <td class="form-label">Date of Birth:</td>
                            <td>
                                <asp:Label ID="lblDateOfBirth" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="form-label">Date Joined:</td>
                            <td>
                                <asp:Calendar ID="calUpdateDateJoined" SelectionMode="Day" ShowGridLines="True" runat="server"></asp:Calendar><br />
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                           <td><br /><asp:Button id="btnUpdateMember" Text="Update Member" runat="server" OnClick="btnUpdateMember_Click" ValidationGroup="UpdateMembers" Enabled="False" /></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td><asp:Label ID="lblUpdateResult" runat="server" /></td>
                        </tr>
                    </table>

                </article>  
            </asp:Panel>

            <asp:Panel ID="pnlReports" runat="server" Visible="false">
                <article>
                    <h2>Reports</h2>

                    <table>
                        <tr>
                            <td class="form-label">Select Report:</td>
                            <td><asp:DropDownList ID="ddReportSelection" runat="server" OnSelectedIndexChanged="ddReportSelection_SelectedIndexChanged" AutoPostBack="True" /></td>
                        </tr>
                    </table>
                    <br />

                    <asp:Chart ID="chrSalaryHistogram" runat="server" Width="800px">
                        <Series>
                            <asp:Series Name="Salary" Legend="Legend1">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
                        </ChartAreas>
                        <Titles>
                            <asp:Title Name="HistogramTitle" Text="Salary Histogram" Font="Microsoft Sans Serif, 14pt">
                            </asp:Title>
                            <asp:Title DockedToChartArea="ChartArea1" Docking="Bottom" Font="Microsoft Sans Serif, 10pt" IsDockedInsideChartArea="False" Name="Title1" Text="Employee Salary ($)">
                            </asp:Title>
                            <asp:Title DockedToChartArea="ChartArea1" Docking="Left" Font="Microsoft Sans Serif, 10pt" IsDockedInsideChartArea="False" Name="Title2" Text="Employee Count">
                            </asp:Title>
                        </Titles>
                    </asp:Chart>

                    <asp:Chart ID="chrAgeHistogram" runat="server" Width="800px">
                        <Series>
                            <asp:Series Name="Age" Legend="Legend1">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
                        </ChartAreas>
                        <Titles>
                            <asp:Title Name="HistogramTitle" Text="Age Histogram" Font="Microsoft Sans Serif, 14pt">
                            </asp:Title>
                            <asp:Title DockedToChartArea="ChartArea1" Docking="Bottom" Font="Microsoft Sans Serif, 10pt" IsDockedInsideChartArea="False" Name="Title1" Text="Employee Age">
                            </asp:Title>
                            <asp:Title DockedToChartArea="ChartArea1" Docking="Left" Font="Microsoft Sans Serif, 10pt" IsDockedInsideChartArea="False" Name="Title2" Text="Employee Count">
                            </asp:Title>
                        </Titles>
                    </asp:Chart>

                    <asp:Panel ID="pnlAboveGoldMean" runat="server" >
                        <h3 class="report-header">Salary Above Gold Class Mean</h3>

                        <asp:DataGrid id="dgAboveGoldMean" runat="server"
                            BorderColor="black"
                            BorderWidth="1"
                            CellPadding="3"
                            AutoGenerateColumns="false" HorizontalAlign="Center">

                          <HeaderStyle BackColor="#00aaaa" />

                          <PagerStyle Mode="NextPrev" />

                          <Columns>
                             <asp:BoundColumn 
                                  HeaderText="Member ID" 
                                  DataField="MembershipId"/>

                            <asp:BoundColumn 
                                  HeaderText="Last Name" 
                                  DataField="LastName"/>

                             <asp:BoundColumn 
                                  HeaderText="First Name" 
                                  DataField="FirstName"/>

                            <asp:BoundColumn 
                                  HeaderText="Membership Class" 
                                  DataField="MemberClass"/>

                            <asp:BoundColumn 
                                  HeaderText="Date Joined" 
                                  DataField="DateJoined"
                                  DataFormatString="{0:d}" />

                            <asp:BoundColumn 
                                  HeaderText="Date of Birth" 
                                  DataField="DateOfBirth"
                                  DataFormatString="{0:d}" />

                            <asp:BoundColumn 
                                  HeaderText="Salary"
                                  DataField="Salary"
                                  DataFormatString="{0:C}" />

                            <asp:BoundColumn 
                                  HeaderText="Gender" 
                                  DataField="Gender">
                                <ItemStyle HorizontalAlign="Center"/>
                            </asp:BoundColumn>

                          </Columns>
                       </asp:DataGrid>
                   </asp:Panel>

                    <asp:Panel ID="pnlTopLoyalty" runat="server" >
                        <h3 class="report-header">Top <asp:Label ID="lblLoyaltyPercent" runat="server" />% Loyalty Percentile</h3>

                        <asp:DataGrid id="dgTopLoyalty" runat="server"
                            BorderColor="black"
                            BorderWidth="1"
                            CellPadding="3"
                            AutoGenerateColumns="false" HorizontalAlign="Center">

                          <HeaderStyle BackColor="#00aaaa" />

                          <PagerStyle Mode="NextPrev" />

                          <Columns>
                             <asp:BoundColumn 
                                  HeaderText="Member ID" 
                                  DataField="MembershipId"/>

                            <asp:BoundColumn 
                                  HeaderText="Last Name" 
                                  DataField="LastName"/>

                             <asp:BoundColumn 
                                  HeaderText="First Name" 
                                  DataField="FirstName"/>

                            <asp:BoundColumn 
                                  HeaderText="Membership Class" 
                                  DataField="MemberClass"/>

                            <asp:BoundColumn 
                                  HeaderText="Date Joined" 
                                  DataField="DateJoined"
                                  DataFormatString="{0:d}" />

                            <asp:BoundColumn 
                                  HeaderText="Date of Birth" 
                                  DataField="DateOfBirth"
                                  DataFormatString="{0:d}" />

                            <asp:BoundColumn 
                                  HeaderText="Salary"
                                  DataField="Salary"
                                  DataFormatString="{0:C}" />

                            <asp:BoundColumn 
                                  HeaderText="Gender" 
                                  DataField="Gender">
                                <ItemStyle HorizontalAlign="Center"/>
                            </asp:BoundColumn>

                          </Columns>
                       </asp:DataGrid>
                   </asp:Panel>


                </article>  
            </asp:Panel>
        </section>
    </form>
    <footer>
        <p>&copy; 2016 Australian Logisitics</p>
    </footer>
</body>
</html>
