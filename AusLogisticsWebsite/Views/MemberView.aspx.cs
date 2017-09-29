using AusLogisticsWebsite.Controllers;
using AusLogisticsWebsite.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace AusLogisticsWebsite.Views
{
    /// <summary>
    /// Project:    SIT322 Distributed Systems - Assignmnet 2
    /// Written By: Chris O'Beirne - Student #211347444
    /// Date:       24/04/16
    /// </summary>

    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            dgViewMembers.PagerStyle.Mode = PagerMode.NumericPages;

            if (!IsPostBack)
            {
                if (Properties.Settings.Default.MigrateCsvEmployees)
                {
                    MigrationController mc = new MigrationController();
                    mc.MigrateCsvEmployees(new MemberController());

                    MemberController memberController = new MemberController();
                    memberController.UpdateMemberClasses();
                }

                _AddGenderDropDownList(ddGender);
                _AddReportDropDownList(ddReportSelection);

                dgViewMembers.DataSource = _CreateViewMembersGridDataSource();
                dgViewMembers.DataBind();
            }

        }

        private void HideAllPanels()
        {
            pnlViewMembers.Visible = false;
            pnlMigrateMembers.Visible = false;
            pnlAddMember.Visible = false;
            pnlRemoveMember.Visible = false;
            pnlUpdateMember.Visible = false;
            pnlReports.Visible = false;
        }

        private void _AddGenderDropDownList(DropDownList dropDown)
        {
            if (dropDown.Items.Count == 0)
            {
                dropDown.Items.Add(new ListItem("F"));
                dropDown.Items.Add(new ListItem("M"));
            }
        }

        private void _AddReportDropDownList(DropDownList dropDown)
        {
            if (dropDown.Items.Count == 0)
            {
                dropDown.Items.Add(new ListItem("Salary Histogram"));
                dropDown.Items.Add(new ListItem("Age Histogram"));
                dropDown.Items.Add(new ListItem("Salary Above Gold Class"));
                dropDown.Items.Add(new ListItem("Top Loyalty Percentile"));
            }
        }


        private ICollection _CreateViewMembersGridDataSource(int memberId = 0, string firstName = "", string lastName = "")
        {
            DataTable dt = new DataTable();
            DataRow dr;

            dt.Columns.Add(new DataColumn("MembershipId", typeof(Int32)));
            dt.Columns.Add(new DataColumn("LastName", typeof(string)));
            dt.Columns.Add(new DataColumn("FirstName", typeof(string)));
            dt.Columns.Add(new DataColumn("MemberClass", typeof(string)));
            dt.Columns.Add(new DataColumn("DateJoined", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("DateOfBirth", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("Salary", typeof(decimal)));
            dt.Columns.Add(new DataColumn("Gender", typeof(char)));

            MemberController mc = new MemberController();
            List<MemberModel> selectMembers;

            if (memberId != 0 || firstName != "" || lastName != "")
            {
                selectMembers = mc.SelectMembers(memberId, firstName, lastName);
            }
            else
            {
                selectMembers = mc.SelectMembers();
            }

            foreach(MemberModel member in selectMembers)
            {
                dr = dt.NewRow();

                dr[0] = member.MembershipId;
                dr[1] = member.LastName;
                dr[2] = member.FirstName;
                dr[3] = member.MemberClassName;
                dr[4] = member.DateJoined;
                dr[5] = member.DateOfBirth;
                dr[6] = member.Salary;
                dr[7] = member.Gender;

                dt.Rows.Add(dr);
            }
           
            DataView dv = new DataView(dt);

            return dv;
        }

        private ICollection _CreateAboveGoldGridDataSource(List<MemberModel> members)
        {
            DataTable dt = new DataTable();
            DataRow dr;

            dt.Columns.Add(new DataColumn("MembershipId", typeof(Int32)));
            dt.Columns.Add(new DataColumn("LastName", typeof(string)));
            dt.Columns.Add(new DataColumn("FirstName", typeof(string)));
            dt.Columns.Add(new DataColumn("MemberClass", typeof(string)));
            dt.Columns.Add(new DataColumn("DateJoined", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("DateOfBirth", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("Salary", typeof(decimal)));
            dt.Columns.Add(new DataColumn("Gender", typeof(char)));

            foreach (MemberModel member in members)
            {
                dr = dt.NewRow();

                dr[0] = member.MembershipId;
                dr[1] = member.LastName;
                dr[2] = member.FirstName;
                dr[3] = member.MemberClassName;
                dr[4] = member.DateJoined;
                dr[5] = member.DateOfBirth;
                dr[6] = member.Salary;
                dr[7] = member.Gender;

                dt.Rows.Add(dr);
            }

            DataView dv = new DataView(dt);

            return dv;
        }

        private ICollection _CreateTopLoyaltyDataSource(List<MemberModel> members)
        {
            DataTable dt = new DataTable();
            DataRow dr;

            dt.Columns.Add(new DataColumn("MembershipId", typeof(Int32)));
            dt.Columns.Add(new DataColumn("LastName", typeof(string)));
            dt.Columns.Add(new DataColumn("FirstName", typeof(string)));
            dt.Columns.Add(new DataColumn("MemberClass", typeof(string)));
            dt.Columns.Add(new DataColumn("DateJoined", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("DateOfBirth", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("Salary", typeof(decimal)));
            dt.Columns.Add(new DataColumn("Gender", typeof(char)));

            foreach (MemberModel member in members)
            {
                dr = dt.NewRow();

                dr[0] = member.MembershipId;
                dr[1] = member.LastName;
                dr[2] = member.FirstName;
                dr[3] = member.MemberClassName;
                dr[4] = member.DateJoined;
                dr[5] = member.DateOfBirth;
                dr[6] = member.Salary;
                dr[7] = member.Gender;

                dt.Rows.Add(dr);
            }

            DataView dv = new DataView(dt);

            return dv;
        }

        protected void lbtnViewMembers_Click(object sender, EventArgs e)
        {
            HideAllPanels();
            pnlViewMembers.Visible = true;

            dgViewMembers.DataSource = _CreateViewMembersGridDataSource();
            dgViewMembers.DataBind();
        }


        protected void lbtnMigrateMember_Click(object sender, EventArgs e)
        {
            HideAllPanels();
            pnlMigrateMembers.Visible = true;
        }

        protected void lbtnAddMember_Click(object sender, EventArgs e)
        {
            HideAllPanels();
            pnlAddMember.Visible = true;
        }

        protected void lbtnRemoveMember_Click(object sender, EventArgs e)
        {
            HideAllPanels();
            pnlRemoveMember.Visible = true;
        }

        protected void lbtnUpdateMember_Click(object sender, EventArgs e)
        {
            HideAllPanels();
            pnlUpdateMember.Visible = true;
        }

        protected void lbtnReports_Click(object sender, EventArgs e)
        {
            HideAllPanels();
            pnlReports.Visible = true;

            SelectReport();
        }

        protected void btAddSubmit_Click(object sender, EventArgs e)
        {
            MemberController controller = new MemberController();

            MemberModel newMember = new MemberModel
            {
                FirstName = tbFirstName.Text,
                LastName = tbLastName.Text,
                DateJoined = DateTime.Now,
                DateOfBirth = calBirthday.SelectedDate,
                Salary = Convert.ToDecimal(tbSalary.Text),
                Gender = Convert.ToChar(ddGender.SelectedValue)
            };

            newMember.MemberClassId = controller.GetMemberClassId(newMember.Salary, newMember.DateJoined);

            bool memberAdded = controller.AddMember(newMember);

            string resultMessage = "";
            if(memberAdded)
            {
                resultMessage = string.Format("{0} {1} successfully added", tbFirstName.Text, tbLastName.Text);
                this.lblAddMemberResult.Text = resultMessage;

                tbFirstName.Text = "";
                tbLastName.Text = "";
                tbSalary.Text = "";
            }
            else
            {
                resultMessage = string.Format("{0} {1} failed to add", tbFirstName.Text, tbLastName.Text);
                this.lblAddMemberResult.Text = resultMessage;
            }
        }

        protected void dgViewMembers_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            dgViewMembers.CurrentPageIndex = e.NewPageIndex;
            dgViewMembers.DataSource = _CreateViewMembersGridDataSource();
            dgViewMembers.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            int findId = (tbFindId.Text.Trim() == "") ? 0 : Convert.ToInt32(tbFindId.Text);
            dgViewMembers.DataSource = _CreateViewMembersGridDataSource(findId, tbFindFirstName.Text, tbFindLastName.Text);
            dgViewMembers.CurrentPageIndex = 0;
            dgViewMembers.DataBind();
        }


        protected void btnMigrateMember_Click(object sender, EventArgs e)
        {
            if (tbMigrateEmployeeNumber.Text != "")
            {
                int empNo = Convert.ToInt32(tbMigrateEmployeeNumber.Text);

                MigrationController migrationController = new MigrationController();
                MemberController memberController = new MemberController();

                List<SalaryModel> salaries = migrationController.SelectEmployeeSalaries();
                decimal salary = migrationController.SelectEmployeeCurrentSalary(empNo, salaries);

                bool migrateOk = migrationController.MigrateCsvEmployee(memberController, salary, empNo);

                string resultMessage = "";
                if (migrateOk)
                {
                    resultMessage = string.Format("{0} {1} successfully migrated", lblMigrateFirstName.Text, lblMigrateLastName.Text);
                    lblMigrateResult.Text = resultMessage;

                    tbMigrateEmployeeNumber.Text = "";
                    lblMigrateFirstName.Text = "";
                    lblMigrateLastName.Text = "";
                    lblMigrateGender.Text = "";
                    lblMigrateDateOfBirth.Text = "";
                }
                else
                {
                    resultMessage = string.Format("{0} {1} migration failed!", lblMigrateFirstName.Text, lblMigrateLastName.Text);
                    lblMigrateResult.Text = resultMessage;
                }

                btnMigrateMember.Enabled = false;
            }
        }

        protected void btnMigrateSearch_Click(object sender, EventArgs e)
        {
            int empNo = (tbMigrateEmployeeNumber.Text.Trim() == "") ? -1 : Convert.ToInt32(tbMigrateEmployeeNumber.Text);

            EmployeeModel employee = null;

            if (empNo != -1)
            {
                MigrationController migrationController = new MigrationController();

                employee = migrationController.SelectCsvEmployee(empNo);
            }

            if (employee != null)
            {
                lblMigrateFirstName.Text = employee.FirstName;
                lblMigrateLastName.Text = employee.LastName;
                lblMigrateGender.Text = employee.Gender.ToString();
                lblMigrateDateOfBirth.Text = employee.BirthDate.ToString("dd/MM/yyyy");

                btnMigrateMember.Enabled = true;
                lblMigrateResult.Text = "";
            }
            else
            {
                lblMigrateResult.Text = "Employee not found!";
            }
        }

        protected void btnRemoveMemberSearch_Click(object sender, EventArgs e)
        {
            int memberNo = (tbRemoveMembershipId.Text.Trim() == "") ? 0 : Convert.ToInt32(tbRemoveMembershipId.Text);

            MemberController memberController = new MemberController();

            MemberModel member = memberController.SelectMember(memberNo);
            
            if(member != null)
            {
                lblRemoveFirstName.Text = member.FirstName;
                lblRemoveLastName.Text = member.LastName;
                lblRemoveGender.Text = member.Gender.ToString();
                lblRemoveDateOfBirth.Text = member.DateOfBirth.ToString("dd/MM/yyyy");

                btnRemoveMember.Enabled = true;
                lblRemoveResult.Text = "";
            }
            else
            {
                lblRemoveResult.Text = "Member not found!";
            }
        }

        protected void btnRemoveMember_Click(object sender, EventArgs e)
        {
            if (tbRemoveMembershipId.Text != "")
            {
                int memberNo = Convert.ToInt32(tbRemoveMembershipId.Text);

                MemberController memberController = new MemberController();

                bool removeOk = memberController.DeleteMember(memberNo);

                string resultMessage = "";
                if (removeOk)
                {
                    resultMessage = string.Format("{0} {1} successfully removed", lblRemoveFirstName.Text, lblRemoveLastName.Text);
                    lblRemoveResult.Text = resultMessage;

                    tbRemoveMembershipId.Text = "";

                    lblRemoveFirstName.Text = "";
                    lblRemoveLastName.Text = "";
                    lblRemoveGender.Text = "";
                    lblRemoveDateOfBirth.Text = "";
                }
                else
                {
                    resultMessage = string.Format("{0} {1} removal failed!", lblRemoveFirstName.Text, lblRemoveLastName.Text);
                    lblRemoveResult.Text = resultMessage;
                }

                btnRemoveMember.Enabled = false;
            }
        }


        protected void btnUpdateSearch_Click(object sender, EventArgs e)
        {
            int memberNo = (tbUpdateMemberId.Text.Trim() == "") ? 0 : Convert.ToInt32(tbUpdateMemberId.Text);

            MemberController memberController = new MemberController();

            MemberModel member = memberController.SelectMember(memberNo);

            if (member != null)
            {
                lblUpdateMembershipId.Text = Convert.ToString(member.MembershipId);
                tbUpdateFirstName.Text = member.FirstName;
                tbUpdateLastName.Text = member.LastName;

                _AddGenderDropDownList(ddUpdateGender);
                ddUpdateGender.SelectedIndex = ddUpdateGender.Items.IndexOf(ddUpdateGender.Items.FindByText(member.Gender.ToString()));

                lblDateOfBirth.Text = string.Format("{0:d}", member.DateOfBirth.Date);

                calUpdateDateJoined.SelectedDate = member.DateJoined.Date;
                calUpdateDateJoined.TodaysDate = member.DateJoined.Date;

                tbUpdateSalary.Text = string.Format("{0}", member.Salary);

                btnUpdateMember.Enabled = true;
                lblUpdateResult.Text = "";
            }
            else
            {
                lblUpdateResult.Text = "Member not found!";
            }
        }

        protected void btnUpdateMember_Click(object sender, EventArgs e)
        {
            if (tbUpdateMemberId.Text != "")
            {
                int memberNo = Convert.ToInt32(tbUpdateMemberId.Text);

                MemberController memberController = new MemberController();

                MemberModel member = memberController.SelectMember(memberNo);

                member.FirstName = tbUpdateFirstName.Text;
                member.LastName = tbUpdateLastName.Text;
                member.DateOfBirth = member.DateOfBirth;
                member.DateJoined = calUpdateDateJoined.SelectedDate;
                member.Salary = Convert.ToDecimal(tbUpdateSalary.Text);
                member.Gender = Convert.ToChar(ddUpdateGender.SelectedValue);


                bool updateOk = memberController.UpdateMember(member);

                string resultMessage = "";
                if (updateOk)
                {
                    resultMessage = string.Format("{0} {1} successfully updated", tbUpdateFirstName.Text, tbUpdateLastName.Text);
                    lblUpdateResult.Text = resultMessage;
                }
                else
                {
                    resultMessage = string.Format("{0} {1} update failed!", tbUpdateFirstName.Text, tbUpdateLastName.Text);
                    lblUpdateResult.Text = resultMessage;
                }
            }
        }

        protected void _UpdateSalaryHistograph()
        {
            chrSalaryHistogram.Visible = true;

            MemberController mc = new MemberController();
            List<MemberModel> members = mc.SelectMembers();

            foreach (var point in SalaryChartReportModel.GetSalaryHistogramPoints(members))
            {
                chrSalaryHistogram.Series["Salary"].Points.Add(point);
            }
        }

        protected void _UpdateAgeHistograph()
        {
            chrAgeHistogram.Visible = true;

            MemberController mc = new MemberController();
            List<MemberModel> members = mc.SelectMembers();

            foreach (var point in AgeChartReportModel.GetAgeHistogramPoints(members))
            {
                chrAgeHistogram.Series["Age"].Points.Add(point);
            }

        }

        protected void _UpdateSalaryAboveGoldMeanList()
        {
            pnlAboveGoldMean.Visible = true;

            MemberController mc = new MemberController();
            List<MemberModel> members = mc.SelectMembers();

            List<MemberModel> reportMembers = AboveGoldMeanReportModel.GetAboveGoldMeanMembers(members, Properties.Settings.Default.GoldTopPercentile);

            dgAboveGoldMean.DataSource = _CreateAboveGoldGridDataSource(reportMembers);
            dgAboveGoldMean.DataBind();

        }

        protected void _UpdateTopLoyaltyList()
        {
            pnlTopLoyalty.Visible = true;
            lblLoyaltyPercent.Text = Convert.ToString(Properties.Settings.Default.TopLoyaltyPercentile);

            MemberController mc = new MemberController();
            List<MemberModel> members = mc.SelectMembers();

            List<MemberModel> reportMembers = TopLoyaltyReportModel.GetTopLoyaltyMemebers(members, Properties.Settings.Default.TopLoyaltyPercentile);

            dgTopLoyalty.DataSource = _CreateAboveGoldGridDataSource(reportMembers);
            dgTopLoyalty.DataBind();

        }

        protected void SelectReport()
        {
            chrAgeHistogram.Visible = false;
            chrSalaryHistogram.Visible = false;
            pnlAboveGoldMean.Visible = false;
            pnlTopLoyalty.Visible = false;

            switch (ddReportSelection.SelectedValue)
            {
                case "Age Histogram":
                    _UpdateAgeHistograph();
                    break;

                case "Salary Histogram":
                    _UpdateSalaryHistograph();
                    break;

                case "Salary Above Gold Class":
                    _UpdateSalaryAboveGoldMeanList();
                    break;

                case "Top Loyalty Percentile":
                    _UpdateTopLoyaltyList();
                    break;


                default:
                    break;
            }
        }

        protected void ddReportSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectReport();
        }
    }
}