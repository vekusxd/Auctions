export const accessTokenId = "ACCESS_TOKEN";
export const refreshTokenId = "REFRESH_TOKEN";

export const getAccessTokenHelper = () => localStorage.getItem(accessTokenId);
export const getRefreshTokenHelper = () => localStorage.getItem(refreshTokenId);

export const setAccessTokenHelper = (token) => {
  localStorage.setItem(accessTokenId, token);
};

export const setRefreshTokenHelper = (token) =>
  localStorage.setItem(refreshTokenId, token);

export const removeAccessTokenHelper = () =>
  localStorage.removeItem(accessTokenId);
export const removeRefreshTokenHelper = () =>
  localStorage.removeItem(refreshTokenId);
