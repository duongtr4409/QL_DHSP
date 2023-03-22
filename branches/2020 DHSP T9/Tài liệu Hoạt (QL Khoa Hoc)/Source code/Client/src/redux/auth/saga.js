import { all, takeEvery, put, fork, call } from "redux-saga/effects";
import { push } from "react-router-redux";
import { getToken, clearToken } from "../../helpers/utility";
import actions from "./actions";
import api from "../../containers/Page/config";
import apisso from "../../customApp/containers/DangNhapSSO/config";
import { message } from "antd";

const fakeApiCall = true; // auth0 or express JWT

export function* loginRequest() {
  yield takeEvery("LOGIN_REQUEST", function* ({ payload }) {
    const response = yield call(api.dangNhap, payload.param);
    if (response.data.Status > 0) {
      const user = response.data.User;
      const Roles = response.data.Roles;
      let role = {};
      yield response.data.ListRole.forEach((value) => {
        role[value.MaChucNang] = {
          view: value.Xem,
          add: value.Them,
          edit: value.Sua,
          delete: value.Xoa,
        };
      });
      if (fakeApiCall) {
        yield put({
          type: actions.LOGIN_SUCCESS,
          token: "secret token",
          profile: "Profile",
          user,
          role,
          Roles
        });
      } else {
        yield put({ type: actions.LOGIN_ERROR });
      }
    }
  });
}
export function* loginSSO() {
  yield takeEvery("LOGIN_SSO_REQUEST", function* ({ payload }) {
    const response = yield call(apisso.DangNhapSSO, payload.param);
    if (response.data.Status > 0) {
      const user = response.data.User;
      const Roles = response.data.Roles;
      let role = {};
      yield response.data.ListRole.forEach((value) => {
        role[value.MaChucNang] = {
          view: value.Xem,
          add: value.Them,
          edit: value.Sua,
          delete: value.Xoa,
        };
      });
      if (fakeApiCall) {
        yield put({
          type: actions.LOGIN_SUCCESS,
          token: "secret token",
          profile: "Profile",
          user,
          role,
          Roles
        });
      } else {
        yield put({ type: actions.LOGIN_ERROR });
      }
    } else {
      message.error(response.data.Message ? response.data.Message : "Đăng nhập thất bại");
      yield put({ type: actions.LOGIN_ERROR });
      clearToken();
      yield put(push("/"));
    }
  });
}

export function* loginSuccess() {
  yield takeEvery(actions.LOGIN_SUCCESS, function* (payload) {
    const username = payload.user.TenNguoiDung;
    yield localStorage.setItem("id_token", payload.token);
    yield localStorage.setItem("user_id", payload.user.NguoiDungID);
    yield localStorage.setItem("access_token", payload.user.Token);
    yield localStorage.setItem("role", JSON.stringify(payload.role));
    yield localStorage.setItem("user", JSON.stringify(payload.user));
    yield localStorage.setItem("list_role", JSON.stringify(payload.Roles));
  });
}

export function* loginError() {
  yield takeEvery(actions.LOGIN_ERROR, function* () {});
}

export function* logout() {
  yield takeEvery(actions.LOGOUT, function* () {
    clearToken();
    yield put(push("/"));
  });
}

export function* checkAuthorization() {
  yield takeEvery(actions.CHECK_AUTHORIZATION, function* () {
    const token = getToken().get("idToken");
    const userId = getToken().get("userId");
    const accessToken = getToken().get("accessToken");
    if (userId && accessToken) {
      const param = { NguoiDungID: userId, Token: accessToken };
      const response = yield call(api.chiTiet, param);
      if (response.data.Status > 0) {
        let user = response.data.User;
        let Roles = response.data.Roles;
        user = { ...user, Token: accessToken };
        let role = {};
        yield response.data.ListRole.forEach((value) => {
          role[value.MaChucNang] = {
            view: value.Xem,
            add: value.Them,
            edit: value.Sua,
            delete: value.Xoa,
          };
        });
        if (token) {
          yield put({
            type: actions.LOGIN_SUCCESS,
            token,
            profile: "Profile",
            user,
            role,
            Roles
          });
        }
      } else {
        clearToken();
      }
    } else {
      clearToken();
    }
  });
}

export default function* rootSaga() {
  yield all([fork(checkAuthorization), fork(loginRequest), fork(loginSuccess), fork(loginError), fork(logout), fork(loginSSO)]);
}
