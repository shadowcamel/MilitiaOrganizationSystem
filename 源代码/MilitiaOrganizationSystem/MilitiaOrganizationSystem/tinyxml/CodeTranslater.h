#include<iostream>
#include<afx.h>

class CodeTranslater {
public:
	static CString UTF8ToGBK(const char szOut[] )
	{
		wchar_t* wszGBK;
		//³¤¶È
		int len = MultiByteToWideChar(CP_UTF8, 0, (LPCSTR)szOut, -1, NULL, 0);
		wszGBK = new wchar_t[len+1];
		memset(wszGBK, 0, len * 2 + 2);
		MultiByteToWideChar(CP_UTF8, 0, (LPCSTR)szOut, -1, (LPWSTR)wszGBK, len);
		CString a;
		a.Append(wszGBK);

		delete []wszGBK;
		return a;
	}
};