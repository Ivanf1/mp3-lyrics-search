import { useEffect, useState } from "react";
import { Status } from "../common/ServiceStatus";
import { SOLR_URL } from "../constants/constants";
import StatusDot from "./StatusDot";

const SolrService = () => {
  const [status, setStatus] = useState<Status>(Status.Loading);

  useEffect(() => {
    const checkStatus = async () => {
      fetch(`${SOLR_URL}select`)
        .then((response) => {
          if (response.status === 503) {
            setStatus(Status.Starting);
          } else if (response.ok) {
            setStatus(Status.Running);
          }
        })
        .catch((_) => {
          setStatus(Status.NotRunning);
        });
    };

    checkStatus();
    let timerId = setInterval(() => checkStatus(), 5000);
    return () => clearInterval(timerId);
  }, []);

  return (
    <div className="service-status-container">
      <div className="service-name">Solr</div>
      <StatusDot state={status} />
      <div className="service-status">
        <button>{status}</button>
      </div>
    </div>
  );
};

export default SolrService;
