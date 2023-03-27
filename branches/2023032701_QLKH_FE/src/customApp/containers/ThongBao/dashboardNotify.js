import React from "react";
import { Carousel, Descriptions, Icon, List, message, Tooltip } from "antd";
import api from "./config";
const SampleNextArrow = (props) => {
  const { className, style, onClick } = props;
  return <Icon type="right" className={`${className} ant-custom-arrow`} style={{ ...style, display: "block" }} onClick={onClick} />;
};

const SamplePrevArrow = (props) => {
  const { className, style, onClick } = props;
  return <Icon type="left" className={`${className} ant-custom-arrow`} style={{ ...style, display: "block" }} onClick={onClick} />;
};
const settings = {
  nextArrow: <SampleNextArrow />,
  prevArrow: <SamplePrevArrow />,
};

export default class GoModal extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      hide: false,
      data: [],
    };
  }
  componentDidMount() {
    this.setState({ data: this.props.data });
  }

  render() {
    const { data } = this.state;

    return (
      <div style={{ maxHeight: 500, marginLeft: -48 }}>
        <Carousel {...settings} arrows>
          {data.map((item, index) => (
            <List.Item.Meta
              title={
                <p>
                  {item.TenThongBao}{" "}
                  <span className="float-right">
                    <Tooltip title="Đã xem">
                      <Icon
                        onClick={() => {
                          api.TatThongBao([{ DoiTuongID: item.DoiTuongID }]).then((res) => {
                            if (res && res.data && res.data.Status === 1) {
                              const dataNew = this.state.data;
                              dataNew.splice(index, 1);
                              if (dataNew.length === 0) {
                                this.props.closeNotify();
                              }
                              this.setState({ data: dataNew });
                            } else {
                              message.error("Thao tác thất bại");
                            }
                          });
                        }}
                        type="check"
                      ></Icon>
                    </Tooltip>
                  </span>
                  <span className="clearfix"></span>
                </p>
              }
              description={
                <div className="mb-3" style={{ maxHeight: 400, overflow: "auto" }}>
                  <p>{item.NoiDung}</p>
                  <div>
                    {item.FileDinhKem.map((file) => (
                      <span className="border border-primary rounded mx-1 my-1 d-inline-block p-1">
                        <a download={file.TenFileGoc} target="_blank" href={file.FileUrl}>
                          {file.TenFileGoc}
                        </a>

                      </span>
                    ))}
                  </div>
                </div>
              }
            />
            // <div>
            //   <div>{item.NoiDung}</div>
            // </div>
          ))}
        </Carousel>
      </div>
    );
  }
}
