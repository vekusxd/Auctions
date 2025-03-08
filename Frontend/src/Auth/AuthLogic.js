import Cookies from "js-cookie";

export const getAccessToken = () => Cookies.get("ACCESS_TOKEN");
export const getRefreshToken = () => Cookies.get("REFRESH_TOKEN");
export const isAuthenticated = () => !!getAccessToken();

export const refreshTokens = async () => {
  const result = await fetch("/api/account/refresh", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      RefreshToken: getRefreshToken(),
    }),
  });
  return await result.json();
};

export const authenticate = async (redirectTo) => {
  if (getRefreshToken()) {
    try {
      const tokens = await refreshTokens();

      const expires = (tokens.expirationMinutes || 15 * 60) * 1000;
      const expiresIn = new Date(new Date().getTime() + expires);

      Cookies.set("ACCESS_TOKEN", tokens.accessToken, { expires: expiresIn });
      Cookies.set("REFRESH_TOKEN", tokens.refreshToken);

      return true;
    } catch {
      redirectTo();
      return false;
    }
  }

  redirectTo();
  return false;
};
