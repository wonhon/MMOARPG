//===================================================
//作    者：WonHon
//创建时间：2019-08-18-13:42:37
//备    注：角色选择界面
//===================================================

namespace SuperBiomass
{
    public partial class SelectRoleForm : UGuiForm
	{
        private ProcedureMenu m_ProcedureMenu;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_BeginGame.onClick.AddListener(OnBeginGame);
            m_CreateRole.onClick.AddListener(OnCreateRole);
        }
        
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_ProcedureMenu = userData as ProcedureMenu;

        }

        private void OnCreateRole()
        {
            m_ProcedureMenu?.Register();
        }

        private void OnBeginGame()
        {
            m_ProcedureMenu?.StartGame();
        }
    }
}