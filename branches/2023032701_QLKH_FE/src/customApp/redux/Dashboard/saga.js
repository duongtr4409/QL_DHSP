import { all, takeEvery, put, call } from "redux-saga/effects";
import actions from "./actions";
import React from "react";

function* getInitData({payload}) {
  try {
    yield put({
      type: actions.DASHBOARD_GET_INIT_DATA_REQUEST_SUCCESS,
      payload: {

      }
    });
  } catch (e) {
    yield put({
      type: actions.DASHBOARD_GET_INIT_DATA_REQUEST_ERROR
    });
  }
}

export default function* rootSaga() {
  yield all([yield takeEvery(actions.DASHBOARD_GET_INIT_DATA_REQUEST, getInitData)]);
}
