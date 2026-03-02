import { jwtDecode } from "jwt-decode";

const decodeJWT = (token) => {
  try {
    return jwtDecode(token);
  } catch {
    return null;
  }
};

export const isTokenExpired = (token) => {
  const decodedToken = jwtDecode(token);
  return !decodedToken?.exp || decodedToken.exp * 1000 < Date.now();
};

export const getUserInfoFromToken = (token) => {
  const decodedToken = jwtDecode(token);

  if (!decodedToken) return null;

  return {
    id: decodedToken.id,
    name: decodedToken.fullName,
    email: decodedToken.email,
    role: decodedToken.role,
    exp: decodedToken.exp,
  };
};
