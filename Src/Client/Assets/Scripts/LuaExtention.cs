using System.Runtime.InteropServices;

namespace XLua.LuaDLL {
    
    public partial class Lua {
        
        [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaopen_sproto_core(System.IntPtr L);

        [MonoPInvokeCallback(typeof(LuaDLL.lua_CSFunction))]
        public static int LoadSproto(System.IntPtr L) {
            return luaopen_sproto_core(L);
        }
    }
}
