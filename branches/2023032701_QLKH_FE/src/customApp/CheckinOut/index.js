import React, { Component } from "react";
import { connect } from "react-redux";
import actions from "../../redux/CheckinOut/actions";
import appActions from "../../../redux/app/actions";
import api from "./config";
import Constants from "../../../settings/constants";
import LayoutWrapper from "../../../components/utility/layoutWrapper";
import PageHeader from "../../../components/utility/pageHeader";
import PageAction from "../../../components/utility/pageAction";
import Box from "../../../components/utility/box";
import Select, { Option } from "../../../components/uielements/select";
import DatePicker from "../../../components/uielements/datePickerFormat";
import { Upload, Icon, message, Row, Col, Input, Form, Modal, Button, Spin } from "antd";
import "./style.css";
import { Link } from "react-router-dom";
import moment from "moment";
import PopoverCustom from "./PopoverCustom";

const { changeCurrent } = appActions;

const { Dragger } = Upload;
const { Item } = Form;
const { ITEM_LAYOUT_HALF, COL_ITEM_LAYOUT_HALF, COL_COL_ITEM_LAYOUT_RIGHT, fileUploadLimit } = Constants;

function getBase64(img, callback) {
  if (!img) {
    return;
  }
  const reader = new FileReader();
  reader.addEventListener("load", () => callback(reader.result));
  reader.readAsDataURL(img);
}

function beforeUpload(file) {
  const isJpgOrPng = file.type === "image/jpeg" || file.type === "image/png";
  if (!isJpgOrPng) {
    message.error("Chỉ được sử dụng định dạng ảnh .jpg hoặc .png");
  }
  const isLt2M = file.size / 1024 / 1024 < fileUploadLimit;
  if (!isLt2M) {
    message.error(`File ảnh phải nhỏ hơn ${fileUploadLimit}MB`);
  }
  return isJpgOrPng && isLt2M;
}

let today = new Date();

class CheckinOut extends Component {
  constructor(props) {
    super(props);
    document.title = "Checkin";
    this.state = {
      imageCMTTruoc: "",
      imageCMTSau: "",
      imageChanDung: "",
      loadingTruoc: false,
      loadingSau: false,
      loadingChanDung: false,
      recognitionTruoc: false,
      recognitionSau: false,
      recognitionChanDung: false,
      today: today,
      dataCMT: {
        HoVaTen: "",
        NgaySinh: "",
        HoKhau: "",
        DienThoai: "",
        SoCMND: "",
        NoiCapCMND: "",
        NgayCapCMND: "",
        GapCanBo: undefined,
        MaThe: "",
        GioVao: "",
        TenCoQuan: "",
      },
      loading: false,
      isCheckOut: false, //Kiểm tra trạng thái hiện tại có phải là checkout hay không
      ThongTinVaoRaID: "",
      checkCheckOut: false, //Kiểm tra đã checkout hay chưa
      isCheckIn: false,
      showCamera: false,
      visiblePopoverMaThe: false,
      visiblePopoverHoTen: false,
      visiblePopoverCMND: false,
      dataPopover: null,
      loadingSearchMaThe: false,
      loadingSearchHoTen: false,
      loadingSearchCMND: false,
      selectIndex: -1,
    };
  }

  //Get initData---------------------------------------------
  componentDidMount = () => {
    this.props.getInitData();
    this.interval = setInterval(() => {
      today = new Date();
      this.setState({ today });
    }, 1000);
  };

  componentWillUnmount() {
    clearInterval(this.interval);
  }

  fillData = (data, type) => {
    // console.log(data);
    const { dataCMT } = this.state;
    dataCMT.HoVaTen = data.name !== "N/A" ? data.name : type === 2 ? dataCMT.HoVaTen : "";
    dataCMT.NgaySinh = data.birthday !== "N/A" ? moment(data.birthday, "DD-MM-YYYY") : type === 2 ? dataCMT.NgaySinh : "";
    dataCMT.HoKhau = data.address !== "N/A" ? data.address : type === 2 ? dataCMT.HoKhau : "";
    dataCMT.SoCMND = data.id !== "N/A" ? data.id : type === 2 ? dataCMT.SoCMND : "";
    dataCMT.NoiCapCMND = data.issue_by !== "N/A" ? data.issue_by : type === 1 ? dataCMT.NoiCapCMND : "";
    dataCMT.NgayCapCMND = data.issue_date !== "N/A" ? moment(data.issue_date, "DD-MM-YYYY") : type === 1 ? dataCMT.NgayCapCMND : "";
    //type: 1 - Mặt trước 2 - Mặt sau
    if (type === 1) {
      if (data.name === "N/A" || data.id === "N/A") {
        message.destroy();
        message.warning("Ảnh CMND mặt trước không hợp lệ");
      }
    } else if (type === 2) {
      if (data.name !== "N/A" || data.id !== "N/A") {
        message.destroy();
        message.warning("Ảnh CMND mặt sau không hợp lệ");
      }
    }
    this.setState({ dataCMT });
  };

  handleUpload = (type, info) => {
    //Type: 1 - CMT trước, 2 - CMT sau, 3 - Chân dung
    if (type === 1) {
      getBase64(info.file.originFileObj, (imageCMTTruoc) => {
        this.setState({
          imageCMTTruoc,
          loadingTruoc: false,
          recognitionTruoc: true,
        });
        api
          .UploadImage({ image: imageCMTTruoc })
          .then((response) => {
            if (response.data.result_code === 200) {
              this.fillData(response.data, type);
              this.setState({
                recognitionTruoc: false,
              });
            } else if (response.data.result_code === 500) {
              this.setState({ recognitionTruoc: false });
              message.destroy();
              message.warning("Ảnh CMND mặt trước không hợp lệ");
            } else {
              this.setState({ recognitionTruoc: false });
              message.destroy();
              message.error(response.data.result_message);
            }
          })
          .catch((error) => {
            this.setState({ recognitionTruoc: false });
            message.destroy();
            message.error(error.toString());
          });
      });
    } else if (type === 2) {
      getBase64(info.file.originFileObj, (imageCMTSau) => {
        this.setState({
          imageCMTSau,
          loadingSau: false,
          recognitionSau: true,
        });
        api
          .UploadImage({ image: imageCMTSau })
          .then((response) => {
            if (response.data.result_code === 200) {
              this.fillData(response.data, type);
              this.setState({ recognitionSau: false });
            } else if (response.data.result_code === 500) {
              this.setState({ recognitionSau: false });
              message.destroy();
              message.warning("Ảnh CMND mặt sau không hợp lệ");
            } else {
              this.setState({ recognitionSau: false });
              message.destroy();
              message.error(response.data.result_message);
            }
          })
          .catch((error) => {
            this.setState({ recognitionSau: false });
            message.destroy();
            message.error(error.toString());
          });
      });
    } else if (type === 3) {
      getBase64(info.file.originFileObj, (imageChanDung) => {
        this.setState({ loadingChanDung: false, imageChanDung });
        if (this.state.imageCMTTruoc === "" && this.state.imageCMTSau === "") {
          this.setState({ recognitionChanDung: true });
          api
            .GetByChanDung({ Base64File: imageChanDung })
            .then((response) => {
              if (response.data.Status > 0) {
                const dataCMT = response.data.Data;
                dataCMT.NgayCapCMND = moment(dataCMT.NgayCapCMND);
                dataCMT.NgaySinh = moment(dataCMT.NgaySinh);
                this.setState({
                  dataCMT,
                  isCheckOut: true,
                  imageCMTTruoc: dataCMT.AnhCMND_MTBase64,
                  imageCMTSau: dataCMT.AnhCMND_MSBase64,
                  recognitionChanDung: false,
                });
              } else {
                this.setState({ recognitionChanDung: false });
                message.destroy();
                message.error(response.data.Message);
              }
            })
            .catch((error) => {
              this.setState({ recognitionChanDung: false });
              message.destroy();
              message.error(error.toString());
            });
        } else {
          const param = {
            image_cmt: this.state.imageCMTTruoc,
            image_cmt2: this.state.imageCMTSau,
            image_live: this.state.imageChanDung,
          };
          this.setState({ recognitionChanDung: true });
          api
            .Verification(param)
            .then((response) => {
              if (response.data.verify_result === 2) {
                this.setState({ isCheckIn: true, recognitionChanDung: false });
              } else {
                message.destroy();
                message.warning("Ảnh chân dung không khớp với ảnh chứng minh thư");
                this.setState({ isCheckIn: false, recognitionChanDung: false });
              }
            })
            .catch((error) => {
              console.log(error, "error verification");
              this.setState({ imageChanDung: "", recognitionChanDung: false });
              message.destroy();
              message.error("Có lỗi xảy ra khi nhận diện khuôn mặt. Vui lòng thử lại");
            });
        }
      });
    }
  };

  b64toBlob = (dataURI) => {
    let fileType = dataURI.split(";")[0].replace("data:", "");
    let byteString = atob(dataURI.split(",")[1]);
    let ab = new ArrayBuffer(byteString.length);
    let ia = new Uint8Array(ab);

    for (let i = 0; i < byteString.length; i++) {
      ia[i] = byteString.charCodeAt(i);
    }
    return new Blob([ab], { type: fileType }); //eg: image/jpg
  };

  inputNumber = (e) => {
    const key = e.charCode;
    if (key < 48 || key > 57) {
      e.preventDefault();
    }
  };

  CheckIn = () => {
    const param = Object.assign({}, this.state.dataCMT);
    param.NgaySinh = param.NgaySinh !== "" ? moment(param.NgaySinh, "DD/MM/YYYY").format("YYYY-MM-DD") : "";
    param.NgayCapCMND = param.NgayCapCMND !== "" ? moment(param.NgayCapCMND, "DD/MM/YYYY").format("YYYY-MM-DD") : "";
    param.AnhChanDungBase64 = this.state.imageChanDung;
    param.AnhCMND_MTBase64 = this.state.imageCMTTruoc;
    param.AnhCMND_MSBase64 = this.state.imageCMTSau;
    delete param.GioVao;
    if (param.GapCanBo === undefined) {
      message.destroy();
      message.warning("Chưa chọn cán bộ cần gặp");
      return;
    }
    // if (param.MaThe === "") {
    //   message.destroy();
    //   message.warning('Chưa nhập mã thẻ');
    //   return;
    // }
    this.setState({ loading: true });
    api
      .Checkin(param)
      .then((response) => {
        if (response.data.Status > 0) {
          const dataCMT = {
            HoVaTen: "",
            NgaySinh: "",
            HoKhau: "",
            DienThoai: "",
            SoCMND: "",
            NoiCapCMND: "",
            NgayCapCMND: "",
            GapCanBo: undefined,
            MaThe: "",
          };
          Modal.success({
            title: "Thông báo",
            content: `Checkin thành công`,
            okText: "Đóng",
            onOk: () => {
              this.setState({
                dataCMT,
                loading: false,
                imageCMTTruoc: "",
                imageCMTSau: "",
                imageChanDung: "",
                isCheckIn: false,
              });
            },
          });
        } else {
          this.setState({ loading: false });
          message.destroy();
          message.error(response.data.Message);
        }
      })
      .catch((error) => {
        this.setState({ loading: false });
        message.destroy();
        message.error(error.toString());
      });
  };

  CheckOut = () => {
    const param = { ThongTinVaoRaID: this.state.dataCMT.ThongTinVaoRaID };
    this.setState({ loading: true });
    api
      .Checkout(param)
      .then((response) => {
        if (response.data.Status > 0) {
          const dataCMT = {
            HoVaTen: "",
            NgaySinh: "",
            HoKhau: "",
            DienThoai: "",
            SoCMND: "",
            NoiCapCMND: "",
            NgayCapCMND: "",
            GapCanBo: undefined,
            MaThe: "",
          };
          Modal.success({
            title: "Thông báo",
            content: "Checkout thành công",
            okText: "Đóng",
            onOk: () => {
              this.setState({
                dataCMT,
                loading: false,
                imageCMTTruoc: "",
                imageCMTSau: "",
                imageChanDung: "",
                isCheckOut: false,
              });
            },
          });
        } else {
          this.setState({ loading: false });
          message.destroy();
          message.error(response.data.Message);
        }
      })
      .catch((error) => {
        this.setState({ loading: false });
        message.destroy();
        message.error(error.toString());
      });
  };

  changeCanBo = (value) => {
    const { dataCMT } = this.state;
    dataCMT.GapCanBo = value;
    this.setState({ dataCMT });
  };

  scanFile = (type) => {
    if (type === 3) {
      const showCamera = !this.state.showCamera;
      this.setState({ showCamera, imageChanDung: "" });
    } else {
      this.Scan(type);
    }
  };

  blurMaThe = (value, type) => {
    if (value === "") {
      return;
    }
    console.log(this.state.selectIndex);
    if (this.state.selectIndex >= 0) {
      return;
    }
    const { isCheckOut, checkCheckOut } = this.state;
    const param = { MaThe: value, LoaiCheckOut: type };
    if (type === 1) {
      this.setState({ loadingSearchMaThe: true });
    } else if (type === 2) {
      this.setState({ loadingSearchCMND: true });
    } else if (type === 3) {
      this.setState({ loadingSearchHoTen: true });
    }
    api
      .GetByMaThe(param)
      .then((response) => {
        if (type === 1) {
          this.setState({ loadingSearchMaThe: false });
        } else if (type === 2) {
          this.setState({ loadingSearchCMND: false });
        } else if (type === 3) {
          this.setState({ loadingSearchHoTen: false });
        }
        if (response.data.Status > 0) {
          // const {imageCMTTruoc, imageCMTSau} = this.state;
          // const dataCMT = response.data.Data;
          // if ((imageCMTTruoc !== "" || imageCMTSau !== "") && type === 1) {
          //   if (!isCheckOut && !checkCheckOut) {
          //     if (dataCMT.length > 0) {
          //       message.destroy();
          //       message.warning('Mã thẻ đã được sử dụng. Xin mời nhập lại mã thẻ khác');
          //       const dataCMTInput = this.state.dataCMT;
          //       dataCMTInput.MaThe = "";
          //       this.setState({dataCMT: dataCMTInput});
          //     }
          //   }
          // } else {
          const dataPopover = response.data.Data;
          if (dataPopover.length > 0) {
            this.setState({ dataPopover });
            if (type === 1) {
              this.setState({ visiblePopoverMaThe: true });
            } else if (type === 2) {
              this.setState({ visiblePopoverCMND: true });
            } else if (type === 3) {
              this.setState({ visiblePopoverHoTen: true });
            }
          }
          //}
        } else {
          // message.destroy();
          // message.info('Không có dữ liệu')
        }
      })
      .catch((error) => {
        message.destroy();
        message.error(error.toString());
      });
  };

  triggerUpload = (type) => {
    let elementID = "";
    switch (type) {
      case 1:
        elementID = "uploadTruoc";
        break;
      case 2:
        elementID = "uploadSau";
        break;
      case 3:
        elementID = "uploadChanDung";
        break;
    }
    //Nếu type = 3 (Chân dung) và đang hiển thị camera thì gọi api lấy ảnh để checkin - checkout
    if (type === 3 && this.state.showCamera) {
      this.setState({ loadingChanDung: true });
      api.GetImageCamera().then((response) => {
        if (response.data.Data) {
          let header = "data:image/jpeg;base64,";
          const imageBase64 = header + response.data.Data;
          this.setState({ imageChanDung: imageBase64, showCamera: false, loadingChanDung: false });
          const { imageCMTTruoc, imageCMTSau } = this.state;
          if (imageCMTTruoc === "" && imageCMTSau === "") {
            this.setState({ recognitionChanDung: true });
            api
              .GetByChanDung({ Base64File: imageBase64 })
              .then((response) => {
                if (response.data.Status > 0) {
                  const dataCMT = response.data.Data;
                  dataCMT.NgayCapCMND = moment(dataCMT.NgayCapCMND);
                  dataCMT.NgaySinh = moment(dataCMT.NgaySinh);
                  this.setState({
                    dataCMT,
                    isCheckOut: true,
                    imageCMTTruoc: dataCMT.AnhCMND_MTBase64,
                    imageCMTSau: dataCMT.AnhCMND_MSBase64,
                    recognitionChanDung: false,
                  });
                } else {
                  this.setState({ recognitionChanDung: false });
                  message.destroy();
                  message.error(response.data.Message);
                }
              })
              .catch((error) => {
                this.setState({ recognitionChanDung: false });
                message.destroy();
                message.error(error.toString());
              });
          } else {
            const param = {
              image_cmt: imageCMTTruoc,
              image_cmt2: imageCMTSau,
              image_live: imageBase64,
            };
            this.setState({ recognitionChanDung: true });
            api
              .Verification(param)
              .then((response) => {
                if (response.data.verify_result === 2) {
                  this.setState({ isCheckIn: true, recognitionChanDung: false });
                } else {
                  message.destroy();
                  message.warning("Ảnh chân dung không khớp với ảnh chứng minh thư");
                  this.setState({ isCheckIn: false, recognitionChanDung: false });
                }
              })
              .catch((error) => {
                console.log(error, "error verification");
                this.setState({ imageChanDung: "", recognitionChanDung: false });
                message.destroy();
                message.error("Có lỗi xảy ra khi nhận diện khuôn mặt. Vui lòng thử lại");
              });
          }
        } else {
          this.setState({ loadingChanDung: false });
          message.destroy();
          message.warning("Không kết nối được với camera");
        }
      });
    } else {
      const elementUpload = document.getElementById(elementID);
      elementUpload.click();
    }
  };

  reload = () => {
    Modal.confirm({
      title: "Thông báo",
      content: "Bạn có muốn hủy tác vụ hiện tại không ?",
      okText: "Có",
      cancelText: "Không",
      onOk: () => {
        this.setState({
          imageCMTTruoc: "",
          imageCMTSau: "",
          imageChanDung: "",
          loadingTruoc: false,
          loadingSau: false,
          loadingChanDung: false,
          recognitionTruoc: false,
          recognitionSau: false,
          recognitionChanDung: false,
          today: today,
          dataCMT: {
            HoVaTen: "",
            NgaySinh: "",
            HoKhau: "",
            DienThoai: "",
            SoCMND: "",
            NoiCapCMND: "",
            NgayCapCMND: "",
            GapCanBo: undefined,
            MaThe: "",
            GioVao: "",
            TenCoQuan: "",
          },
          loading: false,
          isCheckOut: false, //Kiểm tra trạng thái hiện tại có phải là checkout hay không
          ThongTinVaoRaID: "",
          checkCheckOut: false, //Kiểm tra đã checkout hay chưa
          isCheckIn: false,
          showCamera: false,
          visiblePopoverMaThe: false,
          visiblePopoverHoTen: false,
          visiblePopoverCMND: false,
          dataPopover: null,
          loadingSearchMaThe: false,
          loadingSearchHoTen: false,
          loadingSearchCMND: false,
          selectIndex: -1,
        });
      },
    });
  };

  Scan = (type) => {
    // loadingTruoc: false,recognitionTruoc: true
    if (type === 1) {
      this.setState({ loadingTruoc: true });
    } else if (type === 2) {
      this.setState({ loadingSau: true });
    }
    let query =
      "SetParams?device-name=A64&scanmode=scan&paper-size=none&source=Camera&resolution=300&mode=color&imagefmt=jpg&brightness=0&contrast=0&quality=75&swcrop=false&swdeskew=false&front-eject=false&manual-eject=false&duplexmerge=false&remove-blank-page=false&multifeed-detect=false&denoise=false&remove-blackedges=false&recognize-type=none&recognize-lang=default";
    const ip = "127.0.0.1";
    let socket, socketData;
    let Data = {};
    try {
      socket = new WebSocket("ws://" + ip + ":17777/" + query, "webfxscan");
    } catch (e) {
      Data = { status: 0, message: "Không thể truy cập máy quét", data: null };
      return Data;
    }
    socket.onclose = (event) => {
      socket.close();
    };

    socket.onerror = (event) => {
      console.log(event, "onerror");
      return Data;
    };

    socket.onmessage = (event) => {
      let fileName = "";
      if (event.data.includes("status")) {
        fileName = event.data.split(",")[1];
        query = "GetFileData?filename=" + fileName.trim() + "&thumbnail=false" + "&delete=false";
        try {
          socketData = new WebSocket("ws://" + ip + ":17776/" + query, "webfxscan");
        } catch (e) {
          return;
        }
        socketData.onclose = (event) => {
          socketData.close();
        };

        socketData.onmessage = (event) => {
          const reader = new FileReader();
          reader.readAsDataURL(event.data);
          reader.onloadend = () => {
            let base64data = reader.result.split(",")[1];
            base64data = "data:image/jpeg;base64," + base64data;
            if (type === 1) {
              this.setState({ imageCMTTruoc: base64data, loadingTruoc: false, recognitionTruoc: true });
              api
                .UploadImage({ image: base64data })
                .then((response) => {
                  if (response.data.result_code === 200) {
                    this.fillData(response.data, type);
                    this.setState({
                      recognitionTruoc: false,
                    });
                  } else if (response.data.result_code === 500) {
                    this.setState({ recognitionTruoc: false });
                    message.destroy();
                    message.error("Ảnh CMND mặt trước không hợp lệ");
                  } else {
                    this.setState({ recognitionTruoc: false });
                    message.destroy();
                    message.error(response.data.result_message);
                  }
                })
                .catch((error) => {
                  this.setState({ recognitionTruoc: false });
                  message.destroy();
                  message.error(error.toString());
                });
            } else if (type === 2) {
              this.setState({ imageCMTSau: base64data, loadingSau: false, recognitionSau: true });
              api
                .UploadImage({ image: base64data })
                .then((response) => {
                  if (response.data.result_code === 200) {
                    this.fillData(response.data, type);
                    this.setState({ recognitionSau: false });
                  } else if (response.data.result_code === 500) {
                    this.setState({ recognitionSau: false });
                    message.destroy();
                    message.error("Ảnh CMND mặt sau không hợp lệ");
                  } else {
                    this.setState({ recognitionSau: false });
                    message.destroy();
                    message.error(response.data.result_message);
                  }
                })
                .catch((error) => {
                  this.setState({ recognitionSau: false });
                  message.destroy();
                  message.error(error.toString());
                });
            }
          };
        };
      } else if (event.data.includes("error")) {
        this.setState({ loadingTruoc: false, loadingSau: false });
        message.destroy();
        message.warning("Không kết nối được với máy Scan");
      }
    };
  };

  changeValueCMT = (properties, value) => {
    const { dataCMT } = this.state;
    if (properties === "HoVaTen") {
      value = value.toUpperCase();
    }
    dataCMT[properties] = value;
    this.setState({ dataCMT });
  };

  contentPopover = (data, type) => {
    const { selectIndex } = this.state;
    let element = "";
    let widthPopover = 400;
    if (type === 1) {
      element = document.getElementById("txtMaThe");
      widthPopover = element ? element.clientWidth : widthPopover;
    } else if (type === 2) {
      element = document.getElementById("txtCMND");
      widthPopover = element ? element.clientWidth : widthPopover;
    } else if (type === 3) {
      element = document.getElementById("txtHoTen");
      widthPopover = element ? element.clientWidth : widthPopover;
    }
    if (data) {
      return (
        <div>
          {data.map((item, index) => {
            const style = index % 2 !== 0 ? { background: index === selectIndex ? "#40A9FF" : "#cee5fd" } : { background: index === selectIndex ? "#40A9FF" : "" };
            return (
              <div
                className={"hover-content"}
                style={{
                  ...style,
                  width: widthPopover,
                  cursor: "pointer",
                  userSelect: "none",
                  padding: index === 0 ? "5px 12px" : "10px 12px",
                  color: "black",
                }}
                onClick={() => this.loadDataCheckOut(item)}
              >
                <b>{item.HoVaTen}</b>
                <br />
                <i style={{ paddingLeft: 15 }}>{item.SoCMND}</i>
                <br />
                {item.TenCoQuan !== "" ? <i style={{ paddingLeft: 15 }}>{item.TenCoQuan}</i> : ""}
              </div>
            );
          })}
        </div>
      );
    }
  };

  loadDataCheckOut = (data) => {
    const dataCMT = data;
    dataCMT.NgaySinh = dataCMT.NgaySinh ? moment(dataCMT.NgaySinh) : "";
    dataCMT.NgayCapCMND = dataCMT.NgayCapCMND ? moment(dataCMT.NgayCapCMND) : "";
    this.setState({
      selectIndex: -1,
      visiblePopoverMaThe: false,
      visiblePopoverCMND: false,
      visiblePopoverHoTen: false,
      isCheckOut: dataCMT.GioRa === null,
      dataCMT,
      checkCheckOut: dataCMT.GioRa !== null,
      imageCMTTruoc: dataCMT.AnhCMND_MTBase64 ? dataCMT.AnhCMND_MTBase64 : "",
      imageCMTSau: dataCMT.AnhCMND_MSBase64 ? dataCMT.AnhCMND_MSBase64 : "",
      imageChanDung: dataCMT.AnhChanDungBase64 ? dataCMT.AnhChanDungBase64 : "",
    });
  };

  changeVisiblePopoverMaThe = (visible) => {
    if (!visible) {
      this.setState({ visiblePopoverMaThe: visible, selectIndex: -1 });
    }
  };

  changeVisiblePopoverHoTen = (visible) => {
    if (!visible) {
      this.setState({ visiblePopoverHoTen: visible, selectIndex: -1 });
    }
  };

  changeVisiblePopoverCMND = (visible) => {
    if (!visible) {
      this.setState({ visiblePopoverCMND: visible, selectIndex: -1 });
    }
  };

  selectIndex = (e) => {
    const { visiblePopoverMaThe, visiblePopoverCMND, visiblePopoverHoTen, dataPopover } = this.state;
    let { selectIndex } = this.state;
    const keyCode = e.keyCode;
    if (visiblePopoverMaThe || visiblePopoverCMND || visiblePopoverHoTen) {
      if (keyCode === 40 && selectIndex < dataPopover.length - 1) {
        //Down
        selectIndex++;
      } else if (keyCode === 38) {
        //Up
        if (selectIndex > -1) {
          selectIndex--;
        }
      }
      this.setState({ selectIndex });
    }
    if (selectIndex >= 0 && keyCode === 13) {
      this.loadDataCheckOut(dataPopover[selectIndex]);
    }
  };

  render() {
    const user_id = parseInt(localStorage.getItem("user_id"));
    //Props, state
    const { IPCamera, role, roleBaoCao } = this.props;
    const DanhSachCanBo = this.props.DanhSachCanBo ? this.props.DanhSachCanBo : [];
    if (role.view === 0) {
      this.props.history.push("/dashboard");
    }
    const {
      imageCMTTruoc,
      imageCMTSau,
      imageChanDung,
      loadingTruoc,
      loadingSau,
      loadingChanDung,
      dataCMT,
      isCheckOut,
      loading,
      checkCheckOut,
      isCheckIn,
      recognitionTruoc,
      recognitionSau,
      recognitionChanDung,
      showCamera,
      visiblePopoverMaThe,
      dataPopover,
      visiblePopoverHoTen,
      visiblePopoverCMND,
      loadingSearchMaThe,
      loadingSearchCMND,
      loadingSearchHoTen,
    } = this.state;

    const uploadCMTTruoc = (
      <div>
        <p className="ant-upload-drag-icon">
          <Icon type="upload" />
        </p>
        <p className="ant-upload-text">Kéo, thả ảnh CMT mặt trước</p>
      </div>
    );
    const uploadCMTSau = (
      <div>
        <p className="ant-upload-drag-icon">
          <Icon type="upload" />
        </p>
        <p className="ant-upload-text">Kéo, thả ảnh CMT mặt sau</p>
      </div>
    );
    const uploadChanDung = (
      <div>
        <p className="ant-upload-drag-icon">
          <Icon type="upload" />
        </p>
        <p className="ant-upload-text">Kéo, thả ảnh chân dung</p>
      </div>
    );

    //Props dragger
    const props = {
      // action: '',
      multiple: false,
      showUploadList: false,
      beforeUpload: beforeUpload,
      accept: ".jpeg, .jpg, .png",
      // customRequest: dummyRequest
    };
    const propsTruoc = {
      ...props,
      onChange: (info) => {
        let { status } = info.file;
        // if (info.file.type !== 'image/jpeg' && info.file.type !== 'image/png') {
        //   return;
        // }
        // this.setState({loadingTruoc: true});
        // this.handleUpload(1, info);
        if (status === "error") {
          status = "done";
        }
        if (status === "done") {
          this.setState({ loadingTruoc: true });
          this.handleUpload(1, info);
        }
      },
      disabled: loadingTruoc || loadingSau || loadingChanDung,
      id: "uploadTruoc",
    };
    const propsSau = {
      ...props,
      onChange: (info) => {
        let { status } = info.file;
        this.setState({ loadingSau: true });
        if (status === "error") {
          status = "done";
        }
        if (status === "done") {
          this.handleUpload(2, info);
        }
      },
      disabled: loadingTruoc || loadingSau || loadingChanDung,
      id: "uploadSau",
    };
    const propsChanDung = {
      ...props,
      onChange: (info) => {
        let { status } = info.file;
        this.setState({ loadingChanDung: true });
        if (status === "error") {
          status = "done";
        }
        if (status === "done") {
          this.handleUpload(3, info);
        }
      },
      disabled: loadingTruoc || loadingSau || loadingChanDung || showCamera,
      id: "uploadChanDung",
    };

    const valueGioVaoHS = moment(isCheckOut ? dataCMT.GioVao : today).format("HH:mm");
    const valueGioVaoDMY = moment(isCheckOut ? dataCMT.GioVao : today).format("DD/MM/YYYY");

    const valueGioRaHS = checkCheckOut || isCheckOut ? moment(dataCMT.GioRa).format("HH:mm") : "";
    const valueGioRaDMY = checkCheckOut || isCheckOut ? moment(dataCMT.GioRa).format("DD/MM/YYYY") : "";

    const cameraContent = <img id="video" style={{ maxHeight: 150 }} src={IPCamera} alt="" />;

    return (
      <LayoutWrapper>
        <PageHeader>Checkin - Checkout</PageHeader>
        <PageAction>
          {roleBaoCao.view ? (
            <Link to="bao-cao">
              <Button type={"primary"} style={{ height: 30 }} onClick={() => this.props.changeCurrent(["bao-cao"])}>
                Xem danh sách
              </Button>
            </Link>
          ) : (
            ""
          )}
        </PageAction>
        <Box style={{ minHeight: 270, marginBottom: 5, border: "none" }}>
          <div className={"upload-container"}>
            <div className={"upload-div"}>
              <Dragger {...propsTruoc}>{loadingTruoc && imageCMTTruoc === "" ? <Icon type={"loading"} /> : imageCMTTruoc !== "" ? <img src={imageCMTTruoc} alt="avatar" style={{ maxHeight: 150 }} id={"imgTruoc"} /> : uploadCMTTruoc}</Dragger>
              <div className={"button-scan"}>
                <Button icon={"upload"} style={{ backgroundColor: "#ccc", color: "black", fontWeight: "normal !important" }} onClick={() => this.triggerUpload(1)} disabled={loadingTruoc || loadingSau || loadingChanDung}>
                  Chọn file
                </Button>
                <Button
                  icon={"search"}
                  disabled={loadingTruoc || loadingSau || loadingChanDung}
                  onClick={() => this.scanFile(1)}
                  style={{
                    marginLeft: 10,
                    backgroundColor: "#ccc",
                    color: "black",
                    fontWeight: "normal !important",
                  }}
                >
                  Quét file
                </Button>
              </div>
            </div>
            <div className={"upload-div"}>
              <Dragger {...propsSau}>{loadingSau && imageCMTSau === "" ? <Icon type={"loading"} /> : imageCMTSau !== "" ? <img src={imageCMTSau} alt="avatar" style={{ maxHeight: 150 }} /> : uploadCMTSau}</Dragger>
              <div className={"button-scan"}>
                <Button style={{ backgroundColor: "#ccc", color: "black" }} icon={"upload"} onClick={() => this.triggerUpload(2)} disabled={loadingTruoc || loadingSau || loadingChanDung}>
                  Chọn file
                </Button>
                <Button icon={"search"} disabled={loadingTruoc || loadingSau || loadingChanDung} onClick={() => this.scanFile(2)} style={{ marginLeft: 10, backgroundColor: "#ccc", color: "black" }}>
                  Quét file
                </Button>
              </div>
            </div>
            <div className={"upload-div"}>
              <Dragger {...propsChanDung}>
                <div>{loadingChanDung && imageChanDung === "" ? <Icon type={"loading"} /> : imageChanDung !== "" && !showCamera ? <img src={imageChanDung} alt="avatar" style={{ maxHeight: 150 }} /> : showCamera ? cameraContent : uploadChanDung}</div>
              </Dragger>
              <div className={"button-scan"}>
                <Button style={{ backgroundColor: "#ccc", color: "black" }} icon={showCamera ? "picture" : "upload"} onClick={() => this.triggerUpload(3)} disabled={loadingTruoc || loadingSau || loadingChanDung}>
                  {showCamera ? "Chụp ảnh" : "Chọn file"}
                </Button>
                <Button icon={"camera"} disabled={loadingTruoc || loadingSau || loadingChanDung || isCheckIn} onClick={() => this.scanFile(3)} style={{ marginLeft: 10, backgroundColor: "#ccc", color: "black" }}>
                  {!showCamera ? "Mở camera" : "Tắt camera"}
                </Button>
              </div>
            </div>
          </div>
        </Box>
        <Box title={"THÔNG TIN VÀO - RA"} style={{ paddingTop: 5, marginTop: 5, marginBottom: 5, paddingBottom: 5, border: "1px solid #ccc" }}>
          <Form>
            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF}>
                <Item label={"Giờ vào"} {...ITEM_LAYOUT_HALF} className={"datepicker"}>
                  {/*<TimePicker format={'HH:mm'} placeholder={""} style={{width: 100}} value={moment(today)} disabled/>*/}
                  <Input placeholder={""} style={{ width: "30%", maxWidth: 100 }} value={isCheckIn || isCheckOut || checkCheckOut ? valueGioVaoHS : ""} disabled suffix={<Icon type={"clock-circle"} />} />
                  {/*<DatePicker format={'DD/MM/YYYY'} placeholder={""} style={{width: 200, marginLeft: 20}}*/}
                  {/*            value={moment(today)} disabled/>*/}
                  <Input placeholder={""} style={{ width: "cacl(70% - 20px)", marginLeft: 20, maxWidth: 180 }} value={isCheckIn || isCheckOut || checkCheckOut ? valueGioVaoDMY : ""} disabled suffix={<Icon type={"calendar"} />} />
                </Item>
              </Col>
              <Col {...COL_ITEM_LAYOUT_HALF}>
                <Row>
                  <Col {...COL_COL_ITEM_LAYOUT_RIGHT}>
                    <Item label={"Gặp cán bộ"} {...ITEM_LAYOUT_HALF}>
                      <Select style={{ width: "100%" }} placeholder={"Chọn cán bộ cần gặp"} optionFilterProp={"label"} showSearch notFoundContent={"Không có dữ liệu"} value={dataCMT.GapCanBo} onChange={(value) => this.changeCanBo(value)}>
                        {DanhSachCanBo.map((item) => {
                          return (
                            <Option value={item.CanBoID} label={`${item.TenCanBo} ${item.TenCoQuan}`}>
                              {item.TenCanBo} - <i>{item.TenCoQuan}</i>
                            </Option>
                          );
                        })}
                      </Select>
                    </Item>
                  </Col>
                </Row>
              </Col>
            </Row>
            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF}>
                <Item label={"Giờ ra"} {...ITEM_LAYOUT_HALF} className={"datepicker"}>
                  {/*<TimePicker format={'HH:mm'} placeholder={""} style={{width: 100}} value={moment(today)} disabled/>*/}
                  {/*<DatePicker format={'DD/MM/YYYY'} placeholder={""} style={{width: 200, marginLeft: 20}}*/}
                  {/*            value={moment(today)} disabled/>*/}
                  <Input placeholder={""} style={{ width: "30%", maxWidth: 100 }} value={isCheckOut ? moment(today).format("HH:mm") : valueGioRaHS} disabled suffix={<Icon type={"clock-circle"} />} />
                  <Input placeholder={""} style={{ width: "cacl(70% - 20px)", marginLeft: 20, maxWidth: 180 }} value={isCheckOut ? moment(today).format("DD/MM/YYYY") : valueGioRaDMY} disabled suffix={<Icon type={"calendar"} />} />
                </Item>
              </Col>
              <Col {...COL_ITEM_LAYOUT_HALF}>
                <Row>
                  <Col {...COL_COL_ITEM_LAYOUT_RIGHT}>
                    <Item label={"Mã thẻ"} {...ITEM_LAYOUT_HALF}>
                      <PopoverCustom content={this.contentPopover(dataPopover, 1)} visible={visiblePopoverMaThe} trigger={"click"} onVisibleChange={this.changeVisiblePopoverMaThe} placement={"bottomLeft"}>
                        <Input.Search
                          style={{ width: "100%" }}
                          onSearch={(value) => this.blurMaThe(value, 1)}
                          maxLength={20}
                          value={dataCMT.MaThe}
                          onChange={(value) => this.changeValueCMT("MaThe", value.target.value)}
                          id={"txtMaThe"}
                          loading={loadingSearchMaThe}
                          onKeyDown={this.selectIndex}
                          // onBlur={value => this.blurMaThe(value.target.value, 1)}
                        />
                      </PopoverCustom>
                    </Item>
                  </Col>
                </Row>
              </Col>
            </Row>
          </Form>
        </Box>
        <Box
          title={"THÔNG TIN KHÁCH"}
          style={{
            paddingTop: 5,
            paddingBottom: 0,
            marginTop: 5,
            marginBottom: 120,
            border: "1px solid #ccc",
          }}
          className={"div-info"}
        >
          {recognitionTruoc || recognitionSau || recognitionChanDung ? (
            <div className={"div-loading"}>
              <div className={"spin"}>
                <Spin />
              </div>
            </div>
          ) : (
            ""
          )}
          <Form>
            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF}>
                <Item label={"Họ và tên"} {...ITEM_LAYOUT_HALF}>
                  <PopoverCustom content={this.contentPopover(dataPopover, 3)} visible={visiblePopoverHoTen} trigger={"click"} onVisibleChange={this.changeVisiblePopoverHoTen} placement={"bottomLeft"}>
                    <Input.Search style={{ width: "100%" }} value={dataCMT.HoVaTen} onChange={(value) => this.changeValueCMT("HoVaTen", value.target.value)} onSearch={(value) => this.blurMaThe(value, 3)} id={"txtHoTen"} loading={loadingSearchHoTen} onKeyDown={this.selectIndex} />
                  </PopoverCustom>
                </Item>
              </Col>
              <Col {...COL_ITEM_LAYOUT_HALF}>
                <Row>
                  <Col {...COL_COL_ITEM_LAYOUT_RIGHT}>
                    <Item label={"Số CMND"} {...ITEM_LAYOUT_HALF}>
                      <PopoverCustom content={this.contentPopover(dataPopover, 2)} visible={visiblePopoverCMND} trigger={"click"} onVisibleChange={this.changeVisiblePopoverCMND} placement={"bottomLeft"}>
                        <Input.Search style={{ width: "100%" }} value={dataCMT.SoCMND} onChange={(value) => this.changeValueCMT("SoCMND", value.target.value)} maxLength={12} onSearch={(value) => this.blurMaThe(value, 2)} loading={loadingSearchCMND} id={"txtCMND"} onKeyDown={this.selectIndex} />
                      </PopoverCustom>
                    </Item>
                  </Col>
                </Row>
              </Col>
            </Row>
            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF}>
                <Item label={"Cơ quan"} {...ITEM_LAYOUT_HALF}>
                  <Input style={{ width: "100%" }} value={dataCMT.TenCoQuan} onChange={(value) => this.changeValueCMT("TenCoQuan", value.target.value)} />
                </Item>
              </Col>
              <Col {...COL_ITEM_LAYOUT_HALF}>
                <Row>
                  <Col {...COL_COL_ITEM_LAYOUT_RIGHT}>
                    <Item label={"Ngày cấp"} {...ITEM_LAYOUT_HALF}>
                      {/*<Input style={{width: '100%'}} value={dataCMT.NgayCapCMND}/>*/}
                      <DatePicker onChange={(value) => this.changeValueCMT("NgayCapCMND", value)} format={"DD/MM/YYYY"} style={{ width: "100%" }} placeholder={""} value={dataCMT.NgayCapCMND} />
                    </Item>
                  </Col>
                </Row>
              </Col>
            </Row>
            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF}>
                <Item label={"Số điện thoại"} {...ITEM_LAYOUT_HALF}>
                  <Input style={{ width: "100%" }} value={dataCMT.DienThoai} onChange={(value) => this.changeValueCMT("DienThoai", value.target.value)} onKeyPress={this.inputNumber} />
                </Item>
              </Col>
              <Col {...COL_ITEM_LAYOUT_HALF}>
                <Row>
                  <Col {...COL_COL_ITEM_LAYOUT_RIGHT}>
                    <Item label={"Nơi cấp"} {...ITEM_LAYOUT_HALF}>
                      <Input style={{ width: "100%" }} value={dataCMT.NoiCapCMND} onChange={(value) => this.changeValueCMT("NoiCapCMND", value.target.value)} />
                    </Item>
                  </Col>
                </Row>
              </Col>
            </Row>
            <Row>
              <Col {...COL_ITEM_LAYOUT_HALF}>
                <Item label={"Ngày sinh"} {...ITEM_LAYOUT_HALF}>
                  {/*<Input style={{width: '100%'}} value={dataCMT.NgaySinh}/>*/}
                  <DatePicker onChange={(value) => this.changeValueCMT("NgaySinh", value)} format={"DD/MM/YYYY"} style={{ width: "100%" }} placeholder={""} value={dataCMT.NgaySinh} />
                </Item>
              </Col>
              <Col {...COL_ITEM_LAYOUT_HALF}>
                <Row>
                  <Col {...COL_COL_ITEM_LAYOUT_RIGHT}>
                    <Item label={<span>HK thường trú</span>} {...ITEM_LAYOUT_HALF}>
                      <Input style={{ width: "100%" }} value={dataCMT.HoKhau} onChange={(value) => this.changeValueCMT("HoKhau", value.target.value)} />
                    </Item>
                  </Col>
                </Row>
              </Col>
            </Row>
          </Form>
          {checkCheckOut ? <div className={"notice-checkout"}>Khách đã checkout</div> : ""}
        </Box>
        <div className={"div-footer"}>
          <div className={"div-action"}>
            <Button type={"primary"} onClick={this.CheckIn} disabled={isCheckOut || loading || dataCMT.HoVaTen === "" || dataCMT.SoCMND === "" || role.add === 0 || user_id === 1}>
              Checkin
            </Button>
            <Button type={"primary"} onClick={this.CheckOut} disabled={!isCheckOut || loading || role.add === 0 || user_id === 1}>
              Checkout
            </Button>
            <Button icon={"redo"} onClick={this.reload}>
              Tải lại
            </Button>
          </div>
          <div className={"div-action"} style={{ color: "#096DD9", marginTop: 10, fontWeight: "bold" }}>
            <ins>Lưu ý</ins>: Có thể nhập tên, số CMND hoặc Mã thẻ để thực hiện checkout
          </div>
        </div>
      </LayoutWrapper>
    );
  }
}

function mapStateToProps(state) {
  return {
    ...state.CheckinOut,
  };
}

export default connect(mapStateToProps, { ...actions, changeCurrent })(CheckinOut);
