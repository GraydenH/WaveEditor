#pragma once
#pragma comment(lib, "winmm.lib")

#include <Windows.h>

#ifdef __cplusplus
#define EXPORT extern "C" __declspec (dllexport)
#else
#define EXPORT __declspec (dllexport)
#endif

#define INP_BUFFER_SIZE 16384

typedef struct Data {
	DWORD len;
	PBYTE ip;
} Data;

BOOL CALLBACK DlgProc(HWND, UINT, WPARAM, LPARAM);
EXPORT BOOL OpenDialog();
EXPORT BOOL CloseDialog();
EXPORT Data StopRec();
EXPORT WAVEFORMATEX* GetWaveform();
EXPORT BOOL StartRec(int, int); 
EXPORT BOOL PlayPause();
EXPORT BOOL PlayStart(PBYTE, int, int, int);
EXPORT BOOL PlayStop();