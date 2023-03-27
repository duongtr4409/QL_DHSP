import { all, takeEvery, put, call } from "redux-saga/effects";
import api from "../../containers/BaoCao/config";
import actions from "./actions";

function* getInitData() {
  try {

    yield put({
      type: actions.BAOCAO_GET_DATA_REQUEST_SUCCESS,
      payload: {

      }
    });
  } catch (e) {
    yield put({
      type: actions.BAOCAO_GET_DATA_REQUEST_ERROR
    });
  }
}

export default function* rootSaga() {
  yield all([yield takeEvery(actions.BAOCAO_GET_DATA_REQUEST, getInitData)]);
}
