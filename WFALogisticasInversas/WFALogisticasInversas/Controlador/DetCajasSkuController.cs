using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFALogisticasInversas.Modelo;
using WFALogisticasInversas.ViewModel;

namespace WFALogisticasInversas.Controlador
{
    public class DetCajasSkuController
    {
        DB_A3F19C_producccionEntities db = new DB_A3F19C_producccionEntities();

        public bool Crear(List<DatosViewModel> listaItems)
        {
            try
            {
                List<li_detcajasskus> listaTemp = new List<li_detcajasskus>();
                foreach (var item in listaItems)
                {
                    li_detcajasskus detalle = new li_detcajasskus();

                    detalle.CajasLI_Id = db.li_cajas.Where(x => x.Folio.Equals(item.codigocaja)).FirstOrDefault().id;
                    detalle.skus_Id = db.skus.Where(x => x.codigobarras.Equals(item.sku)).FirstOrDefault().id;
                    detalle.Cantidad = item.cantidad;

                    listaTemp.Add(detalle);
                }

                db.li_detcajasskus.AddRange(listaTemp);
                db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<DatosViewModel> BucarDatosCaja(string foliocaja)
        {
            try
            {
                var detallecaja = db.li_detcajasskus.Where(x => x.li_cajas.Folio.Equals(foliocaja)).ToList();

                List<DatosViewModel> listaTemp = new List<DatosViewModel>();

                int folio = 1;
                foreach (var item in detallecaja)
                {
                    DatosViewModel datos = new DatosViewModel();
                    datos.folio = folio;
                    datos.cantidad = item.Cantidad;
                    datos.sku = item.skus.codigobarras;
                    datos.codigocaja = item.li_cajas.Folio;

                    listaTemp.Add(datos);

                    folio++;
                }

                return listaTemp;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool EditarDatosCajas(List<DatosViewModel> listaItems) 
        {
            try
            {
                foreach (var item in listaItems)
                {
                    int idsku = db.skus.Where(x => x.codigobarras.Equals(item.sku)).FirstOrDefault().id;
                    int idcaja = db.li_cajas.Where(x => x.Folio.Equals(item.codigocaja)).FirstOrDefault().id;
                    var detcajassku = db.li_detcajasskus.Where(x => x.skus_Id.Equals(idsku) && x.CajasLI_Id.Equals(idcaja)).FirstOrDefault();

                    if (detcajassku != null)
                    {
                        detcajassku.Cantidad = item.cantidad;                        
                    }
                    else
                    {
                        li_detcajasskus detcajasskutemp = new li_detcajasskus();
                        detcajasskutemp.skus_Id = db.skus.Where(x => x.codigobarras.Equals(item.sku)).FirstOrDefault().id;
                        detcajasskutemp.CajasLI_Id = db.li_cajas.Where(x => x.Folio.Equals(item.codigocaja)).FirstOrDefault().id;
                        detcajasskutemp.Cantidad = item.cantidad;

                        db.li_detcajasskus.Add(detcajasskutemp);
                    }
                }

                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }            
        }
    }
}
