import { all, takeEvery, put, call } from "redux-saga/effects";
import api from "../../containers/DMCapDeTai/config";
import apiThamSo from "../../containers/ThamSoHeThong/config";
import actions from "./actions";

function* getInitData({ payload }) {
  try {
    //data co quan
    const response = yield call(api.danhSachCapDeTai, payload.filterData);

    let resultData = {
      DanhSachCapDeTai: [],
      expandedKeys: [],
    };
    if (response.data.Data) {
      resultData = yield formatData(response.data.Data);
    }

    //return action
    yield put({
      type: actions.COQUAN_GET_INIT_DATA_REQUEST_SUCCESS,
      payload: {
        DanhSachCapDeTai: resultData.DanhSachCapDeTai,

        expandedKeys: resultData.expandedKeys,
      },
    });
  } catch (e) {
    console.log(e);
    yield put({
      type: actions.COQUAN_GET_INIT_DATA_REQUEST_ERROR,
    });
  }
}

function* getList({ payload }) {
  try {
    const response = yield call(api.danhSachCapDeTai, payload.filterData);
    const resultData = yield formatData(response.data.Data);
    yield put({
      type: actions.COQUAN_GET_LIST_REQUEST_SUCCESS,
      payload: {
        DanhSachCapDeTai: resultData.DanhSachCapDeTai,
        expandedKeys: resultData.expandedKeys,
      },
    });
  } catch (e) {
    yield put({
      type: actions.COQUAN_GET_LIST_REQUEST_ERROR,
    });
  }
}

function formatData(DanhSachCapDeTai) {
  let expandedKeys = [];
  const DanhSach = DanhSachCapDeTai.map((value1, index1) => {
    //-------1
    let title1 = value1.Name;
    let key1 = `${index1}`;
    let isLeaf1 = true;
    let children1 = null;
    if (value1.Children) {
      isLeaf1 = false;
      expandedKeys.push(key1);
      children1 = value1.Children.map((value2, index2) => {
        //------2
        let title2 = value2.Name;
        let key2 = `${index1}-${index2}`;
        let isLeaf2 = true;
        let children2 = null;
        if (value2.Children) {
          isLeaf2 = false;
          expandedKeys.push(key2);
          children2 = value2.Children.map((value3, index3) => {
            //------3
            let title3 = value3.Name;
            let key3 = `${index1}-${index2}-${index3}`;
            let isLeaf3 = true;
            let children3 = null;
            return {
              ...value3,
              title: title3,
              key: key3,
              isLeaf: isLeaf3,
              children: children3,
            };
          });
        }
        return {
          ...value2,
          title: title2,
          key: key2,
          isLeaf: isLeaf2,
          children: children2,
        };
      });
    }
    return {
      ...value1,
      title: title1,
      key: key1,
      isLeaf: isLeaf1,
      children: children1,
    };
  });
  return {
    DanhSachCapDeTai: DanhSach,
    expandedKeys,
  };
}

export default function* rootSaga() {
  yield all([yield takeEvery(actions.COQUAN_GET_INIT_DATA_REQUEST, getInitData)]);
  yield all([yield takeEvery(actions.COQUAN_GET_LIST_REQUEST, getList)]);
}
