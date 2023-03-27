const actions = {
  CHECK_AUTHORIZATION: "CHECK_AUTHORIZATION",
  LOGIN_REQUEST: "LOGIN_REQUEST",
  LOGIN_SSO_REQUEST: "LOGIN_SSO_REQUEST",
  LOGOUT: "LOGOUT",
  LOGOUT_SUCCESS: "LOGOUT_SUCCESS",
  LOGIN_SUCCESS: "LOGIN_SUCCESS",
  LOGIN_ERROR: "LOGIN_ERROR",
  checkAuthorization: () => ({
    type: actions.CHECK_AUTHORIZATION,
  }),
  login: (param) => ({
    type: actions.LOGIN_REQUEST,
    payload: { param },
  }),
  loginSSO: (param) => ({
    type: actions.LOGIN_SSO_REQUEST,
    payload: { param },
  }),
  logout: () => ({
    type: actions.LOGOUT,
  }),
};
export default actions;
