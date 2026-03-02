import { getUserInfoFromToken, isTokenExpired } from "../../Utility/jwtUtility";
import { createSlice } from "@reduxjs/toolkit";
const STORAGE_KEYS = {
  TOKEN: "token-mango",
  USER: "user-mango",
};
const getInitialAuthState = () => {
  const storedToken = localStorage.getItem(STORAGE_KEYS.TOKEN);
  const storedUser = localStorage.getItem(STORAGE_KEYS.USER);
  if (
    !storedToken ||
    storedToken == "undefined" ||
    storedToken === "null" ||
    isTokenExpired(storedToken)
  ) {
    localStorage.removeItem(STORAGE_KEYS.TOKEN);
    localStorage.removeItem(STORAGE_KEYS.USER);
    return {
      user: null,
      token: null,
      isAuthenticated: false,
    };
  }

  let user = null;
  if (storedUser && storedUser !== "undefined" && storedUser != "null") {
    try {
      user = JSON.parse(storedUser);
    } catch {
      user = null;
    }
  }

  if (!user) {
    user = getUserInfoFromToken(storedToken);
    if (user) {
      localStorage.setItem(STORAGE_KEYS.USER, JSON.stringify(user));
    }
  }

  return {
    user,
    token: storedToken,
    isAuthenticated: !!(storedToken && user),
  };
};

const authSlice = createSlice({
  name: "auth",
  initialState: { ...getInitialAuthState() },
  reducers: {
    setAuth: (state, action) => {
      const { user, token } = action.payload;
      state.token = token;
      state.user = user;
      state.isAuthenticated = !!(user && token);
      if (token) localStorage.setItem(STORAGE_KEYS.TOKEN, token);
      if (user) localStorage.setItem(STORAGE_KEYS.USER, JSON.stringify(user));
    },
    logOut: (state) => {
      localStorage.removeItem(STORAGE_KEYS.TOKEN);
      localStorage.removeItem(STORAGE_KEYS.USER);
      state.token = null;
      state.user = null;
      state.isAuthenticated = false;
    },
  },
});

export const { setAuth, logOut } = authSlice.actions;
export default authSlice.reducer;
