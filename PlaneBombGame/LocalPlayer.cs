using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneBombGame
{
    internal class LocalPlayer : Player
    {
        public LocalPlayer() { }

        Plane[] planes { get; set; }

        Plane previewPlane;

        bool preViewPlaneIsValidPlace;
        
        bool flashLight = false;

        AiAssistant aiAssistant = new AiAssistant();

        ArrayList attackHistory = new ArrayList();

        public ArrayList GetAttackHistory()
        {
            return attackHistory;
        }
        public AttackPoint NextAttack()
        {
            return new AttackPoint(1, 1);
        }
        public Plane GetPreviewPlane()
        {
            return previewPlane;
        }
        public void UpdatePreviewPlane(int x,int y,int dir,bool isValidPlace)
        {
            if(previewPlane == null)
            {
                previewPlane = new Plane(x,y,dir);
            }
            else
            {
                previewPlane.x = x;
                previewPlane.y = y;
                previewPlane.direction = dir;
            }
            preViewPlaneIsValidPlace = isValidPlace;
        }
        public void UpdatePreviewPlane()
        {
            flashLight = flashLight ^ true;
        }
        public Plane[] GetPlanes()
        {
            if (planes == null)
            {
                planes = new Plane[3];
            }
            return planes;
        }

        public void SetOnePlane(Plane plane, int index)
        {
            if(planes == null)
            {
                planes = new Plane[3];
                planes[0] = plane;
                return;
            }
            
            planes[index] = plane;
        }

        public void AddAttackPoint(AttackPoint attackPoint, string res)
        {
            attackHistory.Add(attackPoint); 
            aiAssistant.AddAttackPoint(attackPoint, res);//向AI助手传送
            aiAssistant.upDateInfo();
        }


        public void SetPlanes(Plane[] planes)
        {
        }

        public void Init()
        {
            throw new NotImplementedException();
        }

        internal void DrawPreviewPlane(Graphics g)
        {
            previewPlane.Draw(g, true, preViewPlaneIsValidPlace, flashLight);
        }

        internal AiAssistant GetAiAssistant()
        {
            return aiAssistant;
        }

        public AiVirtualPlayer GetAiAssistantPlayer()
        {
            return aiAssistant.GetAiVirtualPlayer();
        }

        
    }
}
