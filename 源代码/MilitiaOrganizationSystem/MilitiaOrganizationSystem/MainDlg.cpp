// MainDlg.cpp : 实现文件
//

#include "stdafx.h"
#include "MilitiaOrganizationSystem.h"
#include "MainDlg.h"
#include "XMLFileTree.h"
#include "afxdialogex.h"


// MainDlg 对话框

IMPLEMENT_DYNAMIC(MainDlg, CDialogEx)

MainDlg::MainDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(MainDlg::IDD, pParent)
{
	m_xmlFileTreeDlg = NULL;//赋值为NULL
}

MainDlg::~MainDlg()
{
	if(m_xmlFileTreeDlg != NULL) {
		delete m_xmlFileTreeDlg;
	}
}

void MainDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(MainDlg, CDialogEx)
//	ON_WM_LBUTTONDOWN()
//	ON_WM_MENUSELECT()
	ON_COMMAND(IDBTN_IMPORTGROUPXML, &MainDlg::OnIdbtnImportgroupxml)
END_MESSAGE_MAP()


// MainDlg 消息处理程序




void MainDlg::OnIdbtnImportgroupxml()
{
	// TODO: 在此添加命令处理程序代码
	if(m_xmlFileTreeDlg == NULL) {
		m_xmlFileTreeDlg = new XMLFileTree;
		m_xmlFileTreeDlg->Create(IDD_XMLFileTree, GetDesktopWindow());
	}
	
	m_xmlFileTreeDlg->ShowWindow(SW_NORMAL);

}
