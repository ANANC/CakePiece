local Util = ANF.Util

function Util:GenUnityAction(luaObj, strLuaFunc, ...)
	local tOrgParamArr 	= {...}
	local dwOrgSize		= select('#', ...)
	
	return function(...)
		
		if tOrgParamArr == nil then
			luaObj[strLuaFunc](luaObj, ...)
		else
			local tCurParamArr 	= {...}
			local dwCurSize 	= select('#', ...)
			for i = 1, dwOrgSize do
				tCurParamArr[i + dwCurSize] = tOrgParamArr[i]
			end	
			
			luaObj[strLuaFunc](luaObj, unpack(tCurParamArr, 1, dwOrgSize + dwCurSize))		
		end

	end
end

function Util:GenGlobalClass(strClassName, super)
	local pNewClass = nil
	if strClassName ~= nil then
		pNewClass = class(super)
		self[strClassName] = pNewClass
	end		
	
	return pNewClass
end

function Util:GetClass(strClassName)
	if strClassName ~= nil then
		return self[strClassName]
	end	
	
	return nil
end

function Util:DebugVector3(v3,outputStr)
	print(outputStr.."x:"..v3.x.." y:"..v3.y.." z:"..v3.z)
end