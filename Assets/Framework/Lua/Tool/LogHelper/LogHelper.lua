LogHelper = {}

local LogGrade =
{
    Trace = 1,
    Debug = 2,
    Info = 3,
    Warnning = 4,
    Error = 5
}

function LogHelper:__GetCSLogHelper()
    if self.CSLogHelper == nil then
        self.CSLogHelper = CS.LogHelper
    end
    return self.CSLogHelper
end

function LogHelper:Trace(title,...)
    local csLogHelper = self:__GetCSLogHelper()
    csLogHelper:LuaLog(LogGrade.Trace,title,self:__ToLogString(...))
end

function LogHelper:Debug(title,...)
    local csLogHelper = self:__GetCSLogHelper()
    csLogHelper:LuaLog(LogGrade.Debug,title,self:__ToLogString(...))
end

function LogHelper:Info(title,...)
    local csLogHelper = self:__GetCSLogHelper()
    csLogHelper:LuaLog(LogGrade.Info,title,self:__ToLogString(...))
end

function LogHelper:Warnning(title,...)
    local csLogHelper = self:__GetCSLogHelper()
    csLogHelper:LuaLog(LogGrade.Warnning,title,self:__ToLogString(...))
end

function LogHelper:Error(title,...)
    local csLogHelper = self:__GetCSLogHelper()
    csLogHelper:LuaLog(LogGrade.Error,title,self:__ToLogString(...))
end

function LogHelper:__ToLogString(...)
    local tb = table.pack(...)
    local length = tb.n

    local str

    if length == 0 then
        str = ""
    else
        local nilStr = "nil"
        for index = 1,length do
            local value = tb[index]

            if value == nil then
                tb[index] = nilStr
            elseif type(value) == "table" then
                tb[index] = self:Table2String(value)
            else
                tb[index] = tostring(value)
            end
        end

        str = table.concat( tb, " " )
    end

    return str
end

function LogHelper:Table2String(tb)
    local tbType = type(tb)
    if tbType == "table" then
        local strTb = {}
        local deep = 1
        self:__Table2String(tb,strTb,deep)
        local str = table.concat( strTb, "" )
        return str
    else
        return tostring(tb)
    end
end

function LuaHelper:__Table2String(tb,strTb,deep)
    if deep >= 1000 then
        return
    end

    local strTbLength = #strTb + 1

    for index = 1,deep do
        strTb[strTbLength] = "  "
        strTbLength = strTbLength + 1
    end
    strTb[strTbLength] = "{\n"
    strTbLength = strTbLength + 1

    for key,value in pairs(tb) do
        local keyType = type(key)

        if keyType == "table" then
            deep = deep + 1
            self:__Table2String(tb,strTb,deep)
            strTbLength = #strTb + 1
        else
            for index = 1,deep + 2 do
                strTb[strTbLength] = "  "
                strTbLength = strTbLength + 1
            end

            strTb[strTbLength] = tostring(key)
            strTbLength = strTbLength + 1

            strTb[strTbLength] = "："
            strTbLength = strTbLength + 1

            strTb[strTbLength] = tostring(value)
            strTbLength = strTbLength + 1

            strTb[strTbLength] = "\n"
            strTbLength = strTbLength + 1
        end
    end

    for index = 1,deep do
        strTb[strTbLength] = "  "
        strTbLength = strTbLength + 1
    end
    strTb[strTbLength] = "}\n"
end