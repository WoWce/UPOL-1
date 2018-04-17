
// ex 10Dlg.h : header file
//

#pragma once
#include "afxwin.h"


// Cex10Dlg dialog
class Cex10Dlg : public CDialog
{
// Construction
public:
	Cex10Dlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_EX10_DIALOG };
#endif

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support


// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	CListBox list1;
	CListBox list2;
	afx_msg void OnBnClickedButton1();
	void moveName(CListBox &list1, CListBox &list2);
	afx_msg void OnBnClickedButton2();
	afx_msg void OnLbnDblclkList1();
	afx_msg void OnLbnDblclkList2();
};
