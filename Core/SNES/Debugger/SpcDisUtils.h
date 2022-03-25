#pragma once
#include "stdafx.h"

class DisassemblyInfo;
class SnesConsole;
class LabelManager;
class EmuSettings;
struct SpcState;

class SpcDisUtils
{
public:
	static void GetDisassembly(DisassemblyInfo &info, string &out, uint32_t memoryAddr, LabelManager* labelManager, EmuSettings* settings);
	static int32_t GetEffectiveAddress(DisassemblyInfo &info, SnesConsole *console, SpcState &state);
	static uint8_t GetOpSize(uint8_t opCode);
	static bool IsUnconditionalJump(uint8_t opCode);
	static bool IsConditionalJump(uint8_t opCode);
	static bool IsJumpToSub(uint8_t opCode);
	static bool IsReturnInstruction(uint8_t opCode);
};
