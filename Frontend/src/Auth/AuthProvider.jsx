import { useState, useCallback, useRef } from "react";
import { AuthContext } from "./AuthContext";
import PropTypes from "prop-types";
import {
  getAccessTokenHelper,
  getRefreshTokenHelper,
  setRefreshTokenHelper,
  setAccessTokenHelper,
  removeAccessTokenHelper,
  removeRefreshTokenHelper,
} from "./AuthHelpers";

export const AuthProvider = ({ children }) => {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [accessToken, setAccessToken] = useState(
    getAccessTokenHelper() || null
  );
  const [refreshToken, setRefreshToken] = useState(
    getRefreshTokenHelper() || null
  );

  const updateTokens = (access, refresh) => {
    setAccessTokenHelper(access);
    setRefreshTokenHelper(refresh);
    setAccessToken(access);
    setRefreshToken(refresh);
  };

  const isRefreshing = useRef(false);
  const refreshSubscribers = useRef([]);

  const login = async (values) => {
    const result = await fetch("/api/account/login", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(values),
    });
    if (result.status !== 200) {
      return false;
    }
    const tokens = await result.json();
    const access = tokens.accessToken;
    const refresh = tokens.refreshToken;
    updateTokens(access, refresh);
    setIsLoggedIn(true);
    return true;
  };

  const logOut = () => {
    removeAccessTokenHelper();
    removeRefreshTokenHelper();
    setAccessToken(null);
    setRefreshToken(null);
    setIsLoggedIn(false);
  };

  const tryRefresh = useCallback(async () => {
    if (isRefreshing.current) {
      return new Promise((resolve) => {
        refreshSubscribers.current.push(resolve);
      });
    }

    isRefreshing.current = true;

    const result = await fetch("/api/account/refresh", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        RefreshToken: getRefreshTokenHelper(),
      }),
    });

    if (result.status === 200) {
      const tokens = await result.json();

      const access = tokens.accessToken;
      const refresh = tokens.refreshToken;

      updateTokens(access, refresh);
      setIsLoggedIn(true);

      refreshSubscribers.current.forEach((resolve) => resolve(access));
      refreshSubscribers.current = [];

      isRefreshing.current = false;
      return access;
    }

    isRefreshing.current = false;
    return null;
  }, []);

  return (
    <AuthContext.Provider
      value={{
        isLoggedIn,
        accessToken,
        login,
        logOut,
        tryRefresh,
        refreshToken,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

AuthProvider.propTypes = {
  children: PropTypes.node.isRequired,
};
