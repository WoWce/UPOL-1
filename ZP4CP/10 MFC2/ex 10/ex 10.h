
// ex 10.h : main header file for the PROJECT_NAME application
//

#pragma once

#ifndef __AFXWIN_H__
	#error "include 'stdafx.h' before including this file for PCH"
#endif

#include "resource.h"		// main symbols


// Cex10App:
// See ex 10.cpp for the implementation of this class
//

class Cex10App : public CWinApp
{
public:
	Cex10App();

// Overrides
public:
	virtual BOOL InitInstance();

// Implementation

	DECLARE_MESSAGE_MAP()
};

extern Cex10App theApp;
