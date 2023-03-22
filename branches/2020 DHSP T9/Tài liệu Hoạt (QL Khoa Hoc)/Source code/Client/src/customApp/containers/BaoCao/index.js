import React, { Component } from "react";
import { connect } from "react-redux";
import actions from "../../redux/BaoCao/actions";

import LayoutWrapper from "../../../components/utility/layoutWrapper";
import PageHeader from "../../../components/utility/pageHeader";
import PageAction from "../../../components/utility/pageAction";
import Box from "../../../components/utility/box";
import ReportPanel from "./reportPanel";
import { message, Row, Col, Form, Select, Button, Spin, Divider } from "antd";
import TinhHinhVaKQThucHienNVKH from "./TinhHinhVa_KQ_ThucHienNVNC_KH";
import ThongKeHoatDongNCKHvaCGCN from "./ThongKeHoatDongNCKHvaCGCN";
import ThongKeKQNC from "./ThongKeKQNC";
import api from "./config";
import "./style.css";
import tablePrintCss from "../../../components/tables/tablePrint.style";
import TKNhiemVuKhoaHocCongNghe from "./TKNhiemVuKhoaHocCongNghe";
import DSNhiemVuKhoaHocCongNghe from "./DSNhiemVuKhoaHocCongNghe";
import ThongKeBCTheoChiSoISSN from './ThongKeBCTheoChiSoISSN'
import { getRoleByKey2 } from "../../../helpers/utility";
const reportTypeList = [
  { label: "Thống kê nhiệm vụ khoa học và công nghệ", value: 0 },
  { label: "Danh sách nhiệm vụ khoa học và công nghệ", value: 1 },
  { label: "Tình hình và kết quả thực hiện nhiệm vụ khoa học và công nghệ", value: 2 },
  { label: "Thống kê các hoạt động nghiên cứu khoa học, chuyển giao công nghệ", value: 3 },
  { label: "Thống kê kết quả nghiên cứu", value: 4 },
  { label: "Báo cáo thống kê bài báo khoa học", value: 5 },
];

class BaoCao extends Component {
  fileName = "DownloadFile";
  constructor(props) {
    super(props);
    document.title = "Báo cáo";
    this.formPrint = React.createRef();
    this.state = {
      // reportType: null,
      data: null,
      loading: false,
      loaded: false,
      isExportKQNC: false,
    };
  }

  //Get initData---------------------------------------------
  componentDidMount = () => { };

  TaoBaoCao = (filter) => {
    // console.log(filter);
    this.filter = filter;
    this.setState({ loading: true });
    switch (this.state.reportType) {
      case 0:
        this.fileName = "Thông kê nhiệm vụ khoa học và công nghệ";
        api.BaoCaoThongKeNhiemVuKhoaHoc(filter).then((res) => {
          if (res.data && res.data.Status === 1) {
            this.setState({ loaded: true, loading: false, data: res.data.Data });
          } else {
            this.setState({ loading: false, data: [] });
            message.error("Lấy báo cáo thất bại");
          }
        });
        break;
      case 1:
        this.fileName = "Danh sách nhiệm vụ khoa học và công nghệ";
        api.BaoCaoDanhSachNhiemVuKhoaHoc(filter).then((res) => {
          if (res.data && res.data.Status === 1) {
            this.setState({ loaded: true, loading: false, data: res.data.Data });
          } else {
            this.setState({ loading: false, data: [] });
            message.error("Lấy báo cáo thất bại");
          }
        });
        break;
      case 2:
        this.fileName = "BCTinhHinhVaKQThucHienNVKH";
        api.BCTinhHinhVaKQThucHienNV(filter).then((res) => {
          if (res) {
            if (res.data && res.data.Status === 1) {
              this.setState({ loaded: true, loading: false, data: res.data.Data });
            } else {
              this.setState({ loading: false, data: [] });
              message.error("Lấy báo cáo thất bại");
            }
          } else {
            message.error("Lấy báo cáo thất bại");
          }
        });
        break;
      case 3:
        this.fileName = "BCHoatDongNCKH";
        api.BCHoatDongNCKH(filter).then((res) => {
          if (res) {
            if (res.data && res.data.Status === 1) {
              this.setState({ loaded: true, loading: false, data: res.data.Data });
            } else {
              this.setState({ loading: false, data: [] });
              message.error("Lấy báo cáo thất bại");
            }
          } else {
            message.error("Lấy báo cáo thất bại");
          }
        });
        break;
      case 4:
        this.fileName = "BCThongKeKQNC";
        api.BCThongKeKQNC(filter).then((res) => {
          if (res) {
            if (res.data && res.data.Status === 1) {
              this.setState({ loaded: true, loading: false, data: res.data.Data });
            } else {
              this.setState({ loading: false, data: [] });
              message.error("Lấy báo cáo thất bại");
            }
          } else {
            message.error("Lấy báo cáo thất bại");
          }
        });
        break;
      case 5:
        this.fileName = "BCThongKeBaoCaoBaiBaoKhoaHoc";
        api.ThongKeBCTheoChiSoISSN(filter).then((res) => {
          if (res) {
            if (res.data && res.data.Status === 1) {
              this.setState({ loaded: true, loading: false, data: res.data.Data });
            } else {
              this.setState({ loading: false, data: [] });
              message.error("Lấy báo cáo thất bại");
            }
          } else {
            message.error("Lấy báo cáo thất bại");
          }
        });
        break;

      default:
        message.warning("Vui lòng chọn báo cáo");
        this.setState({ loading: false, data: [] });
        break;
    }
    // this.setState({ loaded: true });
  };

  exportExcel = () => {
    let html, link, blob, url;
    let preHtml = `<html><head><meta charset='utf-8'></head><body>`;
    let postHtml = "</body></html>";

    html = preHtml + this.formPrint.current.innerHTML + postHtml;
    blob = new Blob(["\ufeff", html], {
      type: "application/vnd.ms-excel",
    });
    url = URL.createObjectURL(blob);
    link = document.createElement("A");
    link.href = url;
    link.download = this.fileName + ".xls"; // default name without extension
    document.body.appendChild(link);
    if (navigator.msSaveOrOpenBlob) navigator.msSaveOrOpenBlob(blob, this.fileName + ".xls");
    // IE10-11
    else link.click(); // other browsers
    document.body.removeChild(link);
    return true;
  };

  printReport = () => {
    let html, link, blob, url;
    let preHtml = "<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:word' xmlns='http://www.w3.org/TR/REC-html40'><head><meta charset='utf-8'><title></title></head><body>";
    let postHtml = "</body></html>";
    html = preHtml + this.formPrint.current.innerHTML + postHtml;
    blob = new Blob(["\ufeff", html], {
      type: "application/msword",
    });
    url = "data:application/vnd.ms-word;charset=utf-8," + encodeURIComponent(html);
    link = document.createElement("A");
    link.href = url;
    link.download = this.fileName; // default name without extension
    document.body.appendChild(link);
    if (navigator.msSaveOrOpenBlob) navigator.msSaveOrOpenBlob(blob, this.fileName);
    // IE10-11
    else link.click(); // other browsers
    document.body.removeChild(link);
  };

  printPDF = () => {
    //xoa iframe cu ------
    let oldIframe = document.querySelectorAll("iframe");
    if (oldIframe && oldIframe.length) {
      oldIframe.forEach((element) => {
        element.parentNode.removeChild(element);
      });
    }
    //tao iframe moi -----
    let node = this.formPrint.current.innerHTML;
    let iframe = document.createElement("iframe");
    iframe.style.display = "none";
    document.body.appendChild(iframe); //make document #html in iframe
    iframe.contentWindow.document.open();
    iframe.contentWindow.document.write(node);
    iframe.contentWindow.document.close();
    iframe.contentWindow.focus();
    iframe.contentWindow.print();
  };

  render() {
    const { reportType, loaded } = this.state;

    return (
      <LayoutWrapper>
        <PageHeader>Báo cáo, thống kê</PageHeader>
        <PageAction/>
        <Box>
          <div className="row align-items-center">
            <div className="col-lg-2 col-xl-1 col-12">Chọn báo cáo</div>
            <div className="col-lg-10 col-xl-11 col-12">
              <Select
                onChange={(value) => {
                  this.setState({ reportType: value, loaded: false, data: [] });
                }}
                allowClear
                value={this.state.reportType}
                className="w-100"
                placeholder="Chọn báo cáo"
              >
                {reportTypeList.map((item) => (
                  <Select.Option key={item.value} value={item.value}>
                    {item.label}
                  </Select.Option>
                ))}
              </Select>
            </div>
            <div className="col-12">
              <ReportPanel
                reportType={this.state.reportType}
                loading={this.state.loading}
                loaded={this.state.loaded}
                onCreateReport={this.TaoBaoCao}
                onExport={async () => {
                  if (this.state.reportType === 4) {
                    this.setState({ isExportKQNC: true }, async () => {
                      const result = await this.exportExcel();
                      this.setState({ isExportKQNC: false });
                    });
                  } else {
                    this.exportExcel();
                  }
                }}
                onPrint={async () => {
                  if (this.state.reportType === 4) {
                    this.setState({ isExportKQNC: true }, async () => {
                      const result = await this.printPDF();
                      this.setState({ isExportKQNC: false });
                    });
                  } else {
                    this.printPDF();
                  }
                }}
              // this.printPDF
              />
            </div>
            <Divider/>
            <Spin spinning={this.state.loading} wrapperClassName="w-100">
              <div className="w-100" ref={this.formPrint} id={"form-print"}>
                {reportType === 0 && loaded ? <TKNhiemVuKhoaHocCongNghe filter={this.filter} data={this.state.data} /> : null}
                {reportType === 1 && loaded ? <DSNhiemVuKhoaHocCongNghe filter={this.filter} data={this.state.data} /> : null}
                {reportType === 2 && loaded ? <TinhHinhVaKQThucHienNVKH filter={this.filter} data={this.state.data}/> : null}
                {reportType === 3 && loaded ? <ThongKeHoatDongNCKHvaCGCN filter={this.filter} data={this.state.data}/> : null}
                {reportType === 4 && loaded ? <ThongKeKQNC exporting={this.state.isExportKQNC} filter={this.filter} data={this.state.data} role={this.props.role}/> : null}
                {reportType === 5 && loaded ? <ThongKeBCTheoChiSoISSN filter={this.filter} data={this.state.data} /> : null}
              </div>
            </Spin>
          </div>
        </Box>
      </LayoutWrapper>
    );
  }
}

function mapStateToProps(state) {
  const role4 = getRoleByKey2("ql-toan-truong");
  const role5 = getRoleByKey2("ql-don-vi");
  state.BaoCao.role = { congbo: role4.view || role5.view ? 1 : 0 };
  return {
    ...state.BaoCao,
  };
}

export default connect(mapStateToProps, actions)(BaoCao);
